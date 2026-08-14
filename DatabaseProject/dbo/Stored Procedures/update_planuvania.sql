CREATE   PROCEDURE [dbo].[update_planuvania]
@report_id int = null
AS
BEGIN
update reports1nf_org_info
set 
	planuvania_1 = round(B.sum_cost_agreement, 2),
	planuvania_2 = round(B.sum_cost_agreement * 12.0 * B.inflation, 2),
	planuvania_3 = round(B.sum_cost_agreement * 12.0 * B.inflation * B.contribution_rate, 2) 
from reports1nf_org_info A
join
(
	select
	*,
	(select Q.inflation from current_inflation Q) / 100.0 as inflation,
	(select Q.contribution_rate from reports1nf_org_info Q where Q.report_id = T.report_id) / 100.0 as contribution_rate
	from
	(
		SELECT
		ar.report_id,
		sum(n.cost_agreement) as  sum_cost_agreement
		FROM reports1nf_arenda ar
		LEFT JOIN arenda a ON a.id = ar.id 
		JOIN reports1nf_arenda_notes n on n.report_id = ar.report_id and n.arenda_id = ar.id
		WHERE 1=1
		AND ( @report_id is null or ar.report_id = @report_id )
		AND isnull(a.is_deleted, 0) = 0 
		AND ar.agreement_state = 1
		group by ar.report_id
	) T
) B on B.report_id = A.report_id
END
