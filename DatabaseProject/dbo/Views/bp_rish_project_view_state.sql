
CREATE VIEW [dbo].[bp_rish_project_view_state]
AS
SELECT dbo.bp_rish_project_state.project_id, dbo.Concatenate(dbo.dict_rish_project_state.id) AS state_id, dbo.Concatenate(dbo.dict_rish_project_state.name) AS state_name
FROM dbo.bp_rish_project_state
INNER JOIN dbo.dict_rish_project_state ON dbo.dict_rish_project_state.id = dbo.bp_rish_project_state.state_id
WHERE bp_rish_project_state.exited_on IS NULL
GROUP BY dbo.bp_rish_project_state.project_id