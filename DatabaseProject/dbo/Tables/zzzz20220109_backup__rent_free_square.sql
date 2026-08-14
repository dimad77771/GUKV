CREATE TABLE [dbo].[zzzz20220109_backup__rent_free_square] (
    [id]               INT             IDENTITY (1, 1) NOT NULL,
    [building_id]      INT             NULL,
    [organization_id]  INT             NULL,
    [rent_period_id]   INT             NULL,
    [sqr_free_total]   NUMERIC (15, 3) NULL,
    [sqr_free_korysna] NUMERIC (15, 3) NULL,
    [sqr_free_mzk]     NUMERIC (15, 3) NULL,
    [free_sqr_floors]  VARCHAR (MAX)   NULL,
    [free_sqr_purpose] VARCHAR (MAX)   NULL
);

