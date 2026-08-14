
create view view_dict_rental_rate as 
select rtrim(ltrim(substring(qq.full_name, CHARINDEX(' ', qq.full_name) + 1, len(qq.full_name) - CHARINDEX(' ', qq.full_name)))) as name2, * from dict_rental_rate qq
