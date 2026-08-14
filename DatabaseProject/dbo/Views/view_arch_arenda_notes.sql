
CREATE VIEW [view_arch_arenda_notes]
AS
SELECT
       aan.[archive_id] AS 'note_id'
      ,aan.archive_arenda_link_code
      ,aan.[arenda_id]
      ,dict_balans_purpose_group.name AS 'purpose_group'
      ,dict_balans_purpose.name AS 'purpose'
      ,aan.[purpose_str]
      ,aan.[rent_square]
      ,aan.[modify_date]
      ,aan.[modified_by]
      ,aan.[note]
      ,aan.[rent_rate]
      ,aan.[cost_narah]
      ,aan.[cost_agreement]
      ,aan.[is_deleted]
      ,aan.[del_date]
      ,aan.[cost_expert_total]
      ,aan.[date_expert]
      ,dict_arenda_payment_type.name AS 'payment_type'
FROM
    arch_arenda_notes aan
    LEFT OUTER JOIN dict_balans_purpose_group ON aan.purpose_group_id = dict_balans_purpose_group.id
    LEFT OUTER JOIN dict_balans_purpose ON aan.purpose_id = dict_balans_purpose.id
    LEFT OUTER JOIN dict_arenda_payment_type ON aan.payment_type_id = dict_arenda_payment_type.id
