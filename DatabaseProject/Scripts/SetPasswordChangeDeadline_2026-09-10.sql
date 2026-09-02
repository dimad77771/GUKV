SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @PasswordExpirationDays int = 180;
DECLARE @PasswordChangeDeadline date = CONVERT(date, '20260910', 112);
DECLARE @LastPasswordChangedDate datetime =
    DATEADD(day, -@PasswordExpirationDays, CAST(@PasswordChangeDeadline AS datetime));

IF CONVERT(date, DATEADD(day, @PasswordExpirationDays, @LastPasswordChangedDate)) <> @PasswordChangeDeadline
BEGIN
    RAISERROR(N'Не вдалося розрахувати дату останньої зміни пароля.', 16, 1);
    RETURN;
END;

BEGIN TRANSACTION;

UPDATE dbo.aspnet_Membership
SET LastPasswordChangedDate = @LastPasswordChangedDate;

select * into zzzz20260902_aspnet_Membership from aspnet_Membership;

DECLARE @UpdatedUsers int = @@ROWCOUNT;

SELECT
    @UpdatedUsers AS UpdatedUsers,
    @LastPasswordChangedDate AS LastPasswordChangedDate,
    @PasswordChangeDeadline AS PasswordChangeDeadline,
    DATEADD(day, 1, @PasswordChangeDeadline) AS BlockingStartsOn;

COMMIT TRANSACTION;
