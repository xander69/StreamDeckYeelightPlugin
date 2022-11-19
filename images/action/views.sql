-- dbo.Template_CourtCode source

CREATE OR ALTER VIEW [dbo].[Template_CourtCode]
AS
select d.id, dt.code, te.value, case when dt.code IN (2,74,78,47,23) then dt.code else NULL end as addrreg  from debt d 
left join cf_debt_0 cfd on d.id = cfd.r_entity_id
left join court cr on cr.id = cfd.custom_attr_677
left join address addr on addr.r_entity_id = cr.id 
left join dbo.cf_address_0 cfo on addr.id = cfo.r_entity_id 
left join dict_term dt on cfo.custom_attr_1299 = dt.code 
left join translation_entity te on dt.id = te.r_entity_id and te.r_entity_attribute_id = 12 and te.r_language_id = 1
where cfd.custom_attr_677 is not null 
and addr.r_entity_type_id = 108
and addr.address_type = 30
and dt.r_dict_name_code = 20142;

-- dbo.Template_BackAddress source

CREATE OR ALTER VIEW [dbo].[Template_BackAddress]
as
select cc.id, cc.addrreg, uta.address_region , uta.value 
from template_courtcode cc 
left join dbo.ut_template_attributes_alpha uta on cc.addrreg = uta.address_region or cc.addrreg is null and uta.address_region is null
where uta.name like N'Обратный адр%';

-- dbo.Template_PredVReg source

CREATE OR ALTER VIEW [dbo].[Template_PredVReg]
as
select cc.id, cc.addrreg, uta.address_region , uta.value from template_courtcode cc left join dbo.ut_template_attributes_alpha uta on cc.addrreg = uta.address_region or cc.addrreg is null and uta.address_region is null
where uta.name like N'Представитель в регион%';