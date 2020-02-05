UPDATE dict_rent_period SET is_active = 0
GO

INSERT INTO dict_rent_period 
(id, name, period_start, period_end, period_quarter, period_year, is_active) VALUES 
(28, '2 квартал 2016', '2016-04-01', '2016-06-30', 6, 2016, 1)
GO