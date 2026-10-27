# Assessment card concurrency checks

The tests use the application merge and SQL code directly, on .NET Framework 4.8.
They cover the object and note tabs, all three grids, field conflicts, clearing values,
negative legacy IDs, document date/number pairs, concurrent inserts, deletion, and
rechecking decisions after another save.

```powershell
dotnet test .\Gukv.AssessmentTests\Gukv.AssessmentTests.csproj
```

Without environment variables only the in-memory tests run. SQL and browser tests
are reported as skipped.

To run SQL tests, supply a SQL Server connection with permission to create and drop
databases. The configured database is never modified: every fixture creates its own
`GUKV_AssessmentTests_<GUID>` database and drops only that generated database.

```powershell
$env:GUKV_ASSESSMENT_TEST_CONNECTION = 'Server=.;Integrated Security=True'
dotnet test .\Gukv.AssessmentTests\Gukv.AssessmentTests.csproj
```

For browser coverage, also enable the isolated IIS Express host:

```powershell
$env:GUKV_ASSESSMENT_BROWSER_TESTS = '1'
dotnet test .\Gukv.AssessmentTests\Gukv.AssessmentTests.csproj
```

Browser tests require Windows, .NET Framework 4.8, IIS Express, and installed Google
Chrome. They copy the actual card, save handlers, scripts, and DevExpress assemblies
to an ignored `bin` directory and use their disposable database. Only navigation,
authentication, resource strings, and the data-source wrapper are replaced by a minimal
test host. Two pages share one browser context to check separate editing snapshots
even when the same user opens the card twice. Screenshots and HTML are retained on
failure. Generated connection configuration is removed after the host stops.

The production site can also be checked with the ASP.NET compiler:

```powershell
& "$env:WINDIR\Microsoft.NET\Framework64\v4.0.30319\aspnet_compiler.exe" `
    -v / -p (Resolve-Path .\ReportWebSite)
```

## Save semantics

- Every save compares the original, submitted, and current values. Only changed
  columns are written. Matching concurrent edits are idempotent.
- One operation is atomic: a card save covers the main form and note; a grid save
  covers one insert, update, or delete. Previously saved operations on other tabs are
  independent. SQL locks are released before the conflict dialog opens.
- Street/house and document date/number are indivisible conflict groups, preventing
  hybrid addresses or document keys.
- Complete document keys are checked within one card and one direction. Existing
  legacy drafts with an empty date or number remain editable. A duplicate insert
  with different contents requires explicit reconciliation with the existing document;
  a key-changing update cannot overwrite another row.
- All writers on this page lock the parent card before checking/writing child rows.
  Snapshots and pending decisions are protected with MachineKey and bound to the
  current session and card. Resolving a conflict repeats the database check.
- A deleted row is never silently recreated. Deleting a changed row requires a choice
  between retaining it and deleting its current version.
- The card signature changes only after an actual write, including child row changes.

The business-key check protects operations through this page. Arbitrary SQL imports
or other external writers must enforce the same rule separately.
