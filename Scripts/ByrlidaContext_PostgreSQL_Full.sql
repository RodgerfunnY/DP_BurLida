-- Скрипт создания таблиц ByrlidaContext для PostgreSQL (Supabase).
-- Выполните ПОСЛЕ Scripts/ApplicationDbContext.sql (создаёт Identity и __EFMigrationsHistory).
-- В Supabase: SQL Editor → New query → вставьте скрипт → Run.

-- Таблица __EFMigrationsHistory уже создана скриптом ApplicationDbContext.sql.

-- 1) UserModelData (нет внешних ключей)
CREATE TABLE IF NOT EXISTS "UserModelData" (
    "Id" serial PRIMARY KEY,
    "Email" text NOT NULL,
    "Name" text NOT NULL,
    "Phone" text NOT NULL,
    "Surname" text NOT NULL,
    "IsApproved" boolean NOT NULL DEFAULT false,
    "Role" text NOT NULL DEFAULT ''
);

-- 2) SkladModelData
CREATE TABLE IF NOT EXISTS "SkladModelData" (
    "Id" serial PRIMARY KEY,
    "Category" text NOT NULL,
    "Info" text NOT NULL,
    "NameSubjecte" text NOT NULL,
    "Quantity" int NOT NULL
);

-- 3) BrigadeModelData (FK → UserModelData)
CREATE TABLE IF NOT EXISTS "BrigadeModelData" (
    "Id" serial PRIMARY KEY,
    "NameBrigade" character varying(100) NOT NULL,
    "Technic" character varying(200) NOT NULL,
    "Info" character varying(500) NOT NULL,
    "ResponsibleUserId" int NULL,
    "Driver" character varying(200) NULL,
    "DrillingMasterAssistant" character varying(200) NULL,
    "AssistantMaster" character varying(200) NULL,
    "BrigadeComposition" character varying(500) NULL,
    CONSTRAINT "FK_BrigadeModelData_UserModelData_ResponsibleUserId" FOREIGN KEY ("ResponsibleUserId") REFERENCES "UserModelData" ("Id") ON DELETE SET NULL
);
CREATE INDEX IF NOT EXISTS "IX_BrigadeModelData_ResponsibleUserId" ON "BrigadeModelData" ("ResponsibleUserId");

-- 4) Order (FK → BrigadeModelData)
CREATE TABLE IF NOT EXISTS "Order" (
    "Id" serial PRIMARY KEY,
    "NameClient" character varying(30) NOT NULL,
    "SurnameClient" character varying(30) NOT NULL,
    "Phone" character varying(14) NOT NULL,
    "Area" character varying(50) NOT NULL,
    "District" character varying(50) NOT NULL,
    "City" character varying(1000) NOT NULL DEFAULT '',
    "Diameter" int NOT NULL,
    "PricePerMeter" int NOT NULL,
    "Pump" int NOT NULL,
    "Arrangement" character varying(50) NOT NULL,
    "Info" character varying(1000) NOT NULL,
    "Status" text NOT NULL,
    "MetersCount" int NULL,
    "PumpModel" text NULL,
    "ArrangementDate" timestamp NULL,
    "Contractor" text NULL,
    "Coordinates" character varying(1000) NULL,
    "Sewer" character varying(1000) NULL,
    "DynamicLevel" character varying(1000) NULL,
    "Filter" character varying(1000) NULL,
    "StaticLevel" character varying(1000) NULL,
    "ArrangementDone" character varying(1000) NULL,
    "Depth" character varying(1000) NULL,
    "PumpInstalled" character varying(1000) NULL,
    "IsDrillingInstallment" boolean NOT NULL DEFAULT false,
    "DrillingFirstContribution" int NULL,
    "DrillingFirstPayment" int NULL,
    "DrillingFirstPaymentDueDate" timestamp NULL,
    "DrillingSecondPayment" int NULL,
    "DrillingSecondPaymentDueDate" timestamp NULL,
    "DrillingThirdPayment" int NULL,
    "DrillingThirdPaymentDueDate" timestamp NULL,
    "DrillingFourthPayment" int NULL,
    "DrillingFourthPaymentDueDate" timestamp NULL,
    "IsArrangementInstallment" boolean NOT NULL DEFAULT false,
    "ArrangementFirstContribution" int NULL,
    "ArrangementFirstPayment" int NULL,
    "ArrangementFirstPaymentDueDate" timestamp NULL,
    "ArrangementSecondPayment" int NULL,
    "ArrangementSecondPaymentDueDate" timestamp NULL,
    "ArrangementThirdPayment" int NULL,
    "ArrangementThirdPaymentDueDate" timestamp NULL,
    "ArrangementFourthPayment" int NULL,
    "ArrangementFourthPaymentDueDate" timestamp NULL,
    "BrigadeStatus" character varying(50) NULL,
    "TotalArrangementAmount" character varying(1000) NULL,
    "TotalDrillingAmount" character varying(1000) NULL,
    "CreatedBy" character varying(255) NULL,
    "CreationTimeData" timestamp NOT NULL DEFAULT now(),
    "WorkDate" timestamp NULL,
    "InstallmentEripNumber" character varying(100) NULL,
    "SubstituteArrangementAssistantMaster" character varying(200) NULL,
    "SubstituteArrangementDriver" character varying(200) NULL,
    "SubstituteDrillingAssistantMaster" character varying(200) NULL,
    "SubstituteDrillingDriver" character varying(200) NULL,
    "DrillingBrigadeId" int NULL,
    "ArrangementBrigadeId" int NULL,
    CONSTRAINT "FK_Order_BrigadeModelData_DrillingBrigadeId" FOREIGN KEY ("DrillingBrigadeId") REFERENCES "BrigadeModelData" ("Id") ON DELETE NO ACTION,
    CONSTRAINT "FK_Order_BrigadeModelData_ArrangementBrigadeId" FOREIGN KEY ("ArrangementBrigadeId") REFERENCES "BrigadeModelData" ("Id") ON DELETE NO ACTION
);
CREATE INDEX IF NOT EXISTS "IX_Order_DrillingBrigadeId" ON "Order" ("DrillingBrigadeId");
CREATE INDEX IF NOT EXISTS "IX_Order_ArrangementBrigadeId" ON "Order" ("ArrangementBrigadeId");

-- 5) OrderComment (FK → Order)
CREATE TABLE IF NOT EXISTS "OrderComment" (
    "Id" serial PRIMARY KEY,
    "Text" character varying(2000) NOT NULL,
    "CreatedAt" timestamp NOT NULL DEFAULT now(),
    "CreatedBy" character varying(255) NULL,
    "OrderId" int NOT NULL,
    "IsDone" boolean NOT NULL DEFAULT false,
    "DoneAt" timestamp NULL,
    CONSTRAINT "FK_OrderComment_Order_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Order" ("Id") ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS "IX_OrderComment_OrderId" ON "OrderComment" ("OrderId");

-- 6) OrderInstallmentPaymentStatus (FK → Order)
CREATE TABLE IF NOT EXISTS "OrderInstallmentPaymentStatus" (
    "Id" serial PRIMARY KEY,
    "OrderId" int NOT NULL,
    "SlotKey" character varying(50) NOT NULL,
    "IsPaid" boolean NOT NULL DEFAULT false,
    "PaidAt" timestamp NULL,
    CONSTRAINT "FK_OrderInstallmentPaymentStatus_Order_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Order" ("Id") ON DELETE CASCADE
);
CREATE UNIQUE INDEX IF NOT EXISTS "IX_OrderInstallmentPaymentStatus_OrderId_SlotKey" ON "OrderInstallmentPaymentStatus" ("OrderId", "SlotKey");

-- 7) Notification (FK → Order опционально)
CREATE TABLE IF NOT EXISTS "Notification" (
    "Id" serial PRIMARY KEY,
    "Message" character varying(500) NOT NULL,
    "RecipientEmail" character varying(255) NOT NULL,
    "CreatedAt" timestamp NOT NULL DEFAULT now(),
    "IsRead" boolean NOT NULL DEFAULT false,
    "OrderId" int NULL,
    CONSTRAINT "FK_Notification_Order_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Order" ("Id") ON DELETE NO ACTION
);
CREATE INDEX IF NOT EXISTS "IX_Notification_OrderId" ON "Notification" ("OrderId");
CREATE INDEX IF NOT EXISTS "IX_Notification_RecipientEmail_IsRead" ON "Notification" ("RecipientEmail", "IsRead");

-- 8) DeviceToken
CREATE TABLE IF NOT EXISTS "DeviceToken" (
    "Id" serial PRIMARY KEY,
    "UserEmail" character varying(255) NOT NULL,
    "Token" character varying(2048) NOT NULL,
    "Platform" character varying(50) NOT NULL,
    "CreatedAt" timestamp NOT NULL DEFAULT now(),
    "UpdatedAt" timestamp NOT NULL DEFAULT now()
);
CREATE UNIQUE INDEX IF NOT EXISTS "IX_DeviceToken_Token" ON "DeviceToken" ("Token");
CREATE INDEX IF NOT EXISTS "IX_DeviceToken_UserEmail_Platform" ON "DeviceToken" ("UserEmail", "Platform");

-- Пометить миграции ByrlidaContext как применённые (чтобы приложение не запускало их снова)
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES
    ('20260303081637_AddOrderExtraFields', '9.0.7'),
    ('20260303102054_AddStaticDynamicFilterFields', '9.0.7'),
    ('20260303103152_AddDepthPumpInstalledArrangementDone', '9.0.7'),
    ('20260303115903_ExtendCityLength', '9.0.7'),
    ('20260303120558_IncreaseTextFieldsTo1000', '9.0.7'),
    ('20260303122130_SyncModelAfterFix', '9.0.7'),
    ('20260303123046_AddOrderCreatedBy', '9.0.7'),
    ('20260303134419_AddUserRoleAndIsApproved', '9.0.7'),
    ('20260303135540_AddUserIdentety', '9.0.7'),
    ('20260304170135_AddOrderTotalsAndBrigadeStatus', '9.0.7'),
    ('20260304173301_AddOrderInstallmentsDueDates', '9.0.7'),
    ('20260304174713_UpdateOrderInstallmentsWithErip', '9.0.7'),
    ('20260306085405_AddBrigadeCompositionAndOrderSubstitutes', '9.0.7'),
    ('20260309065706_AddBrigadeDriverAndAssistant', '9.0.7'),
    ('20260309070544_AddOrderComments', '9.0.7'),
    ('20260309102647_AddOrderInstallmentPaymentStatus', '9.0.7'),
    ('20260309105833_AddNotificationTable', '9.0.7'),
    ('20260312072234_AddDeviceTokenTable', '9.0.7')
ON CONFLICT ("MigrationId") DO NOTHING;
