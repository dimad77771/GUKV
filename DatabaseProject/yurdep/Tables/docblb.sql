CREATE TABLE [yurdep].[docblb] (
    [doc_id] INT   NOT NULL,
    [blb]    IMAGE NULL,
    [txt]    TEXT  NULL,
    CONSTRAINT [docblb_x] PRIMARY KEY CLUSTERED ([doc_id] ASC)
);

