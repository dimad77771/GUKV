CREATE TABLE [dbo].[dict_obj_rights] (
    [id]                               INT           NOT NULL,
    [name]                             VARCHAR (50)  NULL,
    [modified_by]                      VARCHAR (128) NULL,
    [modify_date]                      DATE          NULL,
    [is_active]                        BIT           NULL,
    [enable_org_from]                  BIT           NULL,
    [enable_org_to]                    BIT           NULL,
    [balans_ownership_type_mapping_id] INT           NOT NULL,
    CONSTRAINT [PK__dict_obj__3213E83F3225F076] PRIMARY KEY CLUSTERED ([id] ASC)
);

