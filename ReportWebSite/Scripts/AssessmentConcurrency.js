/* global ASPxClientControl, AssessmentEditState, AssessmentSave */
var AssessmentConcurrency = (function () {
    "use strict";
    var busy = 0, pending = null, sentCard = null, focusBeforeDialog = null, pendingBuilding = null;
    var fields = {
        addr_district_id: ["Assessment_addr_district_id", "int"],
        expert_obj_type_id: ["Assessment_expert_obj_type_id", "int"],
        addr_street_id: ["addr_street_id", "int"], addr_nomer: ["ComboBuilding", "text"],
        org_balans_id: ["Assessment_org_balans_id", "int"], org_renter_id: ["Assessment_org_renter_id", "int"],
        expert_id: ["Assessment_expert_id", "int"], obj_square: ["Assessment_obj_square", "decimal"],
        cost_prim: ["Assessment_cost_prim", "decimal"], is_archived: ["Assessment_is_archived", "bool"],
        arch_num: ["Assessment_arch_num", "text"], final_date: ["Assessment_final_date", "date"],
        note_text: ["AssessmentMemo", "text"]
    };
    var grids = { expert_input_doc: "GridViewAssessmentInputDocs", expert_note_detail: "GridViewAssessmentDetails", expert_output_doc: "GridViewAssessmentOutputDocs" };

    function element(id) { return document.getElementById(id); }
    function control(name) { return ASPxClientControl.GetControlCollection().GetByName(name); }
    function setBusy(change) {
        busy = Math.max(0, busy + change);
        element("assessmentBusy").style.display = busy ? "block" : "none";
    }
    function status(message, error) {
        var node = element("assessmentSaveStatus");
        node.textContent = message;
        node.className = "assessment-save-status" + (error ? " assessment-error" : "");
    }
    function dateString(value) {
        function two(n) { return n < 10 ? "0" + n : String(n); }
        return value.getFullYear() + "-" + two(value.getMonth() + 1) + "-" + two(value.getDate());
    }
    function readCard() {
        var values = {};
        Object.keys(fields).forEach(function (name) {
            var field = fields[name], editor = control(field[0]), value;
            if (!editor) throw new Error("Не знайдено редактор: " + name);
            if (field[1] === "bool") value = editor.GetChecked() ? "1" : "0";
            else if (field[1] === "date") value = editor.GetDate() ? dateString(editor.GetDate()) : null;
            else if (field[1] === "text") value = editor.GetText();
            else value = editor.GetValue();
            if (value !== null && value !== undefined) {
                value = String(value).trim();
                if (!value) value = null;
                else if (field[1] === "decimal") value = Number(value).toFixed(2);
            } else value = null;
            values[name] = value;
        });
        return values;
    }
    function applyCard(response) {
        if (!response.Snapshot) return;
        var current = readCard(), sent = sentCard || current, values = response.Values;
        var addressChanged = current.addr_street_id !== sent.addr_street_id || current.addr_nomer !== sent.addr_nomer;
        Object.keys(fields).forEach(function (name) {
            // Preserve anything typed while this request was in flight.
            if (current[name] !== sent[name] || (addressChanged && (name === "addr_street_id" || name === "addr_nomer"))) return;
            var field = fields[name], editor = control(field[0]), value = values[name];
            if (field[1] === "bool") editor.SetChecked(value === "1");
            else if (field[1] === "date") {
                var parts = value && value.split("-");
                editor.SetDate(parts ? new Date(Number(parts[0]), Number(parts[1]) - 1, Number(parts[2])) : null);
            } else if (field[1] === "text") editor.SetText(value || "");
            else {
                editor.SetValue(value === null ? null : Number(value));
                if (response.DisplayValues && Object.prototype.hasOwnProperty.call(response.DisplayValues, name)) editor.SetText(response.DisplayValues[name]);
            }
        });
        AssessmentEditState.Set("card", response.Snapshot);
        if (!addressChanged && current.addr_street_id !== values.addr_street_id) {
            pendingBuilding = { street: values.addr_street_id, house: values.addr_nomer };
            control("ComboBuilding").PerformCallback(values.addr_street_id || "");
        }
    }
    function cell(row, value) {
        var node = document.createElement("td");
        node.textContent = value == null ? "(порожньо)" : value;
        row.appendChild(node);
        return node;
    }
    function showConflicts(response) {
        if (!pending) focusBeforeDialog = document.activeElement;
        pending = response;
        var body = element("assessmentConflictRows");
        while (body.firstChild) body.removeChild(body.firstChild);
        element("assessmentConflictCaption").textContent = response.Caption;
        element("assessmentConflictError").textContent = "";
        response.Conflicts.forEach(function (conflict, index) {
            var row = document.createElement("tr");
            cell(row, conflict.Caption); cell(row, conflict.Original); cell(row, conflict.Current); cell(row, conflict.Mine);
            var select = document.createElement("select");
            select.id = "assessmentChoice" + index;
            select.setAttribute("aria-label", "Значення для поля: " + conflict.Caption);
            select.add(new Option("Оберіть…", ""));
            select.add(new Option(conflict.Key === "__delete" ? "Зберегти рядок" : "Зараз у базі", "current"));
            if (conflict.CanUseMine) select.add(new Option(conflict.Key === "__delete" ? "Видалити рядок" : "Моє значення", "mine"));
            cell(row, "").appendChild(select);
            body.appendChild(row);
        });
        element("assessmentConflictDialog").style.display = "block";
        var first = element("assessmentChoice0");
        if (first) first.focus();
    }
    function close() {
        if (busy) return;
        pending = null;
        element("assessmentConflictDialog").style.display = "none";
        if (focusBeforeDialog && document.contains(focusBeforeDialog)) focusBeforeDialog.focus();
        focusBeforeDialog = null;
    }
    function result(raw, resolved) {
        var response = typeof raw === "string" ? JSON.parse(raw) : raw;
        if (response.Error) { status(response.Error, true); if (pending) element("assessmentConflictError").textContent = response.Error; return; }
        if (response.Pending) { showConflicts(response); return; }
        if (resolved) close();
        if (response.Signature) {
            var signature = control("AssessmentSignature");
            if (signature) signature.SetText(response.Signature);
        }
        if (response.Table === "expert_note") applyCard(response);
        if (resolved) {
            var gridName = grids[response.Table];
            if (gridName) control(gridName).PerformCallback("resolved");
        }
        status(response.Message || (response.Changed ? "Зміни збережено." : "Немає змін для збереження."), false);
    }
    function save() {
        if (busy || pending) return;
        try {
            sentCard = readCard();
            setBusy(1);
            AssessmentSave.PerformCallback(JSON.stringify({ Action: "save", Snapshot: AssessmentEditState.Get("card"), Values: sentCard }));
        } catch (error) { setBusy(-1); status(error.message, true); }
    }
    function resolve() {
        if (busy || !pending) return;
        var choices = {};
        for (var i = 0; i < pending.Conflicts.length; i++) {
            var select = element("assessmentChoice" + i);
            if (!select.value) { element("assessmentConflictError").textContent = "Оберіть значення для кожного конфлікту."; select.focus(); return; }
            choices[pending.Conflicts[i].Key] = select.value;
        }
        setBusy(1);
        AssessmentSave.PerformCallback(JSON.stringify({ Action: "resolve", Pending: pending.Pending, Choices: choices }));
    }
    function callbackComplete(s, e) {
        setBusy(-1);
        try { result(e.result, !!pending); } catch (error) { status("Помилка обробки відповіді. Введені дані залишилися у формі.", true); }
    }
    function callbackError(s, e) {
        setBusy(-1); e.handled = true;
        if (s === control("ComboBuilding")) pendingBuilding = null;
        status("Не вдалося виконати запит. Введені дані залишилися у формі; повторіть спробу.", true);
    }
    function gridReady(s) {
        if (s.cpEditSnapshot) {
            AssessmentEditState.Set(s.cpStateKey + ":edit", s.cpEditSnapshot);
            delete s.cpEditSnapshot;
        }
        if (s.cpRowSnapshots) {
            Object.keys(s.cpRowSnapshots).forEach(function (id) { AssessmentEditState.Set(s.cpStateKey + ":row:" + id, s.cpRowSnapshots[id]); });
            delete s.cpRowSnapshots;
        }
    }
    function gridEnd(s) {
        setBusy(-1);
        gridReady(s);
        if (s.cpSaveResult) {
            var response = s.cpSaveResult;
            delete s.cpSaveResult;
            try { result(response, false); } catch (error) { status("Помилка обробки відповіді. Повторіть спробу.", true); }
        }
    }
    function streetChanged(s) {
        pendingBuilding = null;
        control("ComboBuilding").SetValue(null);
        control("ComboBuilding").PerformCallback(s.GetValue() == null ? "" : String(s.GetValue()));
    }
    function buildingEnd(s) {
        setBusy(-1);
        var street = control("addr_street_id").GetValue();
        if (pendingBuilding && (street == null ? "" : String(street)) === (pendingBuilding.street || "")) {
            s.SetValue(pendingBuilding.house);
            s.SetText(pendingBuilding.house || "");
        }
        pendingBuilding = null;
    }
    document.addEventListener("keydown", function (event) {
        if (!pending) return;
        if (event.key === "Escape") { event.preventDefault(); close(); }
        if (event.key === "Tab") {
            var items = element("assessmentConflictDialog").querySelectorAll("select, button"), first = items[0], last = items[items.length - 1];
            if (event.shiftKey && document.activeElement === first) { event.preventDefault(); last.focus(); }
            else if (!event.shiftKey && document.activeElement === last) { event.preventDefault(); first.focus(); }
        }
    });
    return { save: save, resolve: resolve, close: close, callbackComplete: callbackComplete, callbackError: callbackError,
        gridReady: gridReady, gridBegin: function () { setBusy(1); }, gridEnd: gridEnd,
        streetChanged: streetChanged, buildingEnd: buildingEnd };
}());
