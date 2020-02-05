-- Street name: {2}
BEGIN

DECLARE @main_street_id int = {0};
DECLARE @dup_street_id int = {1};

--table arch_organizations
update arch_organizations set addr_street_id = @main_street_id 
where addr_street_id = @dup_street_id;

update arch_organizations set phys_addr_street_id = @main_street_id 
where phys_addr_street_id = @dup_street_id;

--table arch_buildings
update arch_buildings set addr_street_id = @main_street_id 
where addr_street_id = @dup_street_id;

--table arch_balans
update arch_balans set obj_street_id = @main_street_id 
where obj_street_id = @dup_street_id;

--table unverified_balans
update unverified_balans set addr_street_id = @main_street_id 
where addr_street_id = @dup_street_id;

--table reports1nf_balans_deleted
update reports1nf_balans_deleted set obj_street_id = @main_street_id 
where obj_street_id = @dup_street_id;

--table reports1nf_balans
update reports1nf_balans set obj_street_id = @main_street_id 
where obj_street_id = @dup_street_id;

--table reports1nf_org_info
update reports1nf_org_info set phys_addr_street_id = @main_street_id 
where phys_addr_street_id = @dup_street_id;

--table reports1nf_buildings
update reports1nf_buildings set addr_street_id = @main_street_id 
where addr_street_id = @dup_street_id;

update reports1nf_buildings set addr_street_id2 = @main_street_id 
where addr_street_id2 = @dup_street_id;

--table dict_expert
update dict_expert set addr_street_id = @main_street_id 
where addr_street_id = @dup_street_id;

--table buildings
update buildings set addr_street_id = @main_street_id 
where addr_street_id = @dup_street_id;

update buildings set addr_street_id2 = @main_street_id 
where addr_street_id2 = @dup_street_id;

--table organizations
update organizations set addr_street_id = @main_street_id 
where addr_street_id = @dup_street_id;

update organizations set phys_addr_street_id = @main_street_id 
where phys_addr_street_id = @dup_street_id;

--table balans
update balans set obj_street_id = @main_street_id 
where obj_street_id = @dup_street_id;

update balans set obj_street_id2 = @main_street_id 
where obj_street_id2 = @dup_street_id;

END