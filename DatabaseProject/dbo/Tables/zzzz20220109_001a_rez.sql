CREATE TABLE [dbo].[zzzz20220109_001a_rez] (
    [F]         INT             NOT NULL,
    [A_addrnom] NVARCHAR (4000) NOT NULL,
    [B_addrnom] NVARCHAR (4000) NOT NULL,
    CONSTRAINT [PK_zzzz20220109_001a_rez] PRIMARY KEY CLUSTERED ([A_addrnom] ASC, [B_addrnom] ASC)
);

