
CREATE VIEW [view_arenda_notes]
AS
SELECT
       an.id AS 'note_id'
      ,an.[arenda_id]
      ,dict_balans_purpose_group.name AS 'purpose_group'
      ,dict_balans_purpose.name AS 'purpose'
      ,an.[purpose_str]
      ,an.[rent_square]
      ,an.[modify_date]
      ,an.[modified_by]
      ,an.[note]
      ,an.[rent_rate]
      ,an.[cost_narah]
      ,an.[cost_agreement]
      ,an.[is_deleted]
      ,an.[del_date]
      ,an.[cost_expert_total]
      ,an.[date_expert]
      ,dict_arenda_payment_type.name AS 'payment_type'
FROM
    arenda_notes an
    LEFT OUTER JOIN dict_balans_purpose_group ON an.purpose_group_id = dict_balans_purpose_group.id
    LEFT OUTER JOIN dict_balans_purpose ON an.purpose_id = dict_balans_purpose.id
    LEFT OUTER JOIN dict_arenda_payment_type ON an.payment_type_id = dict_arenda_payment_type.id
