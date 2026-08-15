# GUKV UI tests

Отдельный проект браузерных тестов для адресной части `ReportWebSite`. Он не подключён
к старому `GUKV_v20_new.sln` и собирается современным .NET независимо от WebForms-проекта.

## Что уже проверяется

- страница входа и вход обычным либо мастер-паролем;
- визуальное совпадение адреса в карточке, списке балансодержателя и Центре;
- одна строка для нормализованной группы номеров;
- объединение объектов всех `building_id` группы без повторов `balans_id`;
- очистка старых объектов после смены улицы;
- отдельный защищённый write-сценарий:
  `выбор -> Зберегти -> Центр не изменился -> Надіслати -> Центр совпал`;
- отсутствие изменения контрольного второго объекта.

## Первичная установка

```powershell
cd D:\Projects\DKVSOURCESFINALEDITION_v20\Gukv.UiTests
dotnet restore
dotnet build
pwsh .\bin\Debug\net8.0\playwright.ps1 install chromium
```

Пароли и connection string в файлы проекта не записываются.

## Общие переменные

```powershell
$env:GUKV_E2E_BASE_URL = 'http://localhost:6670/'
$env:GUKV_E2E_USERNAME = 'имя существующего пользователя'
$env:GUKV_E2E_PASSWORD = 'обычный или master_password'
$env:GUKV_E2E_REPORT_ID = '2098'
$env:GUKV_E2E_BALANS_ID = '6228'
```

Проверка входа и визуального совпадения адресов:

```powershell
dotnet test --filter 'TestCategory=ReadOnly'
```

Без логина можно запустить только проверку наличия элементов страницы входа:

```powershell
dotnet test --filter 'FullyQualifiedName~LoginPageIsRendered'
```

Для запуска с видимым браузером:

```powershell
$env:HEADED = '1'
dotnet test --filter 'TestCategory=ReadOnly'
```

## Сценарий группы эквивалентных номеров

Выберите существующую тестовую группу. Тест автоматически обходит все страницы внутренней
таблицы picker.

```powershell
$env:GUKV_E2E_GROUP_STREET_ID = '123'
$env:GUKV_E2E_GROUP_REPRESENTATIVE_ID = '456'
$env:GUKV_E2E_GROUP_DISPLAY = '33-35'
$env:GUKV_E2E_GROUP_BALANS_IDS = '1001,1002,1003'

dotnet test --filter 'FullyQualifiedName~EquivalentGroupIsShownOnce'
```

## Write-тест

Запускайте только на копии базы. Тест действительно вызывает `Зберегти` и `Надіслати`.
По умолчанию он пытается вернуть исходный адрес, но архивные записи и новая дата отправки
останутся, поэтому это не тест для PROD.

Connection string для `Microsoft.Data.SqlClient` при локальном сертификате обычно требует
`TrustServerCertificate=True`.

```powershell
$env:GUKV_E2E_SQL_CONNECTION = 'Server=.;Database=GUKV20260329;...;TrustServerCertificate=True'
$env:GUKV_E2E_EXPECT_DATABASE = 'GUKV20260329'
# Нужен только если тест запускается не из этого репозитория:
# $env:GUKV_E2E_WEB_CONFIG = 'D:\path\to\ReportWebSite\Web.config'
$env:GUKV_E2E_WRITE_TARGET_BUILDING_ID = '123456'
$env:GUKV_E2E_GUARD_BALANS_ID = '6229' # необязательно, но желательно
$env:GUKV_E2E_RESTORE_AFTER_WRITE = 'true'
$env:GUKV_E2E_ALLOW_WRITES = 'YES_I_AM_USING_A_TEST_DATABASE'

dotnet test --filter 'TestCategory=Write'
```

Предохранитель требует loopback-адрес сайта, правильную строку `GUKV_E2E_ALLOW_WRITES`,
а также совпадение SQL Server и базы у контрольного подключения и локального
`ReportWebSite\Web.config`. Обе базы должны точно совпасть с `GUKV_E2E_EXPECT_DATABASE`.
