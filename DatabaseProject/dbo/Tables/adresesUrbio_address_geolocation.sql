CREATE TABLE [dbo].[adresesUrbio_address_geolocation] (
    [address_id] UNIQUEIDENTIFIER NOT NULL,
    [rownpp]     INT              NOT NULL,
    [lat]        DECIMAL (38, 15) NOT NULL,
    [lon]        DECIMAL (38, 15) NOT NULL,
    PRIMARY KEY CLUSTERED ([address_id] ASC, [rownpp] ASC),
    FOREIGN KEY ([address_id]) REFERENCES [dbo].[adresesUrbio_address] ([address_id])
);

