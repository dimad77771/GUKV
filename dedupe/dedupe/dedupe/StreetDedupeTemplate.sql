-- Street name: {2}
BEGIN

DECLARE @dup_street_id int = {0};

delete from dict_streets where id = @dup_street_id;
delete from dict_1nf_streets where id = @dup_street_id;

END