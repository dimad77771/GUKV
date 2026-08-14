CREATE TABLE [dbo].[adresesUrbio_street_geolocation] (
    [street_id] UNIQUEIDENTIFIER NOT NULL,
    [rownpp]    INT              NOT NULL,
    [lat]       DECIMAL (38, 15) NOT NULL,
    [lon]       DECIMAL (38, 15) NOT NULL,
    PRIMARY KEY CLUSTERED ([street_id] ASC, [rownpp] ASC),
    FOREIGN KEY ([street_id]) REFERENCES [dbo].[adresesUrbio_street] ([street_id])
);

