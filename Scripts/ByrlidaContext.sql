CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303081637_AddOrderExtraFields') THEN
    ALTER TABLE "Order" ADD "MetersCount" int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303081637_AddOrderExtraFields') THEN
    ALTER TABLE "Order" ADD "PumpModel" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303081637_AddOrderExtraFields') THEN
    ALTER TABLE "Order" ADD "ArrangementDate" datetime2;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303081637_AddOrderExtraFields') THEN
    ALTER TABLE "Order" ADD "Contractor" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303081637_AddOrderExtraFields') THEN
    ALTER TABLE "Order" ADD "Coordinates" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303081637_AddOrderExtraFields') THEN
    ALTER TABLE "Order" ADD "Sewer" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303081637_AddOrderExtraFields') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260303081637_AddOrderExtraFields', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303102054_AddStaticDynamicFilterFields') THEN
    ALTER TABLE "Order" ALTER COLUMN "Sewer" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303102054_AddStaticDynamicFilterFields') THEN
    ALTER TABLE "Order" ALTER COLUMN "PumpModel" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303102054_AddStaticDynamicFilterFields') THEN
    ALTER TABLE "Order" ALTER COLUMN "Coordinates" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303102054_AddStaticDynamicFilterFields') THEN
    ALTER TABLE "Order" ALTER COLUMN "Contractor" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303102054_AddStaticDynamicFilterFields') THEN
    ALTER TABLE "Order" ADD "DynamicLevel" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303102054_AddStaticDynamicFilterFields') THEN
    ALTER TABLE "Order" ADD "Filter" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303102054_AddStaticDynamicFilterFields') THEN
    ALTER TABLE "Order" ADD "StaticLevel" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303102054_AddStaticDynamicFilterFields') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260303102054_AddStaticDynamicFilterFields', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303103152_AddDepthPumpInstalledArrangementDone') THEN
    ALTER TABLE "Order" ADD "ArrangementDone" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303103152_AddDepthPumpInstalledArrangementDone') THEN
    ALTER TABLE "Order" ADD "Depth" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303103152_AddDepthPumpInstalledArrangementDone') THEN
    ALTER TABLE "Order" ADD "PumpInstalled" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303103152_AddDepthPumpInstalledArrangementDone') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260303103152_AddDepthPumpInstalledArrangementDone', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303115903_ExtendCityLength') THEN
    ALTER TABLE "Order" ALTER COLUMN "City" TYPE nvarchar(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303115903_ExtendCityLength') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260303115903_ExtendCityLength', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303120558_IncreaseTextFieldsTo1000') THEN
    ALTER TABLE "Order" ALTER COLUMN "StaticLevel" TYPE nvarchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303120558_IncreaseTextFieldsTo1000') THEN
    ALTER TABLE "Order" ALTER COLUMN "Sewer" TYPE nvarchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303120558_IncreaseTextFieldsTo1000') THEN
    ALTER TABLE "Order" ALTER COLUMN "PumpInstalled" TYPE nvarchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303120558_IncreaseTextFieldsTo1000') THEN
    ALTER TABLE "Order" ALTER COLUMN "Filter" TYPE nvarchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303120558_IncreaseTextFieldsTo1000') THEN
    ALTER TABLE "Order" ALTER COLUMN "DynamicLevel" TYPE nvarchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303120558_IncreaseTextFieldsTo1000') THEN
    ALTER TABLE "Order" ALTER COLUMN "Depth" TYPE nvarchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303120558_IncreaseTextFieldsTo1000') THEN
    ALTER TABLE "Order" ALTER COLUMN "Coordinates" TYPE nvarchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303120558_IncreaseTextFieldsTo1000') THEN
    ALTER TABLE "Order" ALTER COLUMN "City" TYPE nvarchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303120558_IncreaseTextFieldsTo1000') THEN
    ALTER TABLE "Order" ALTER COLUMN "Info" TYPE nvarchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303120558_IncreaseTextFieldsTo1000') THEN
    ALTER TABLE "Order" ALTER COLUMN "ArrangementDone" TYPE nvarchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303120558_IncreaseTextFieldsTo1000') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260303120558_IncreaseTextFieldsTo1000', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303122130_SyncModelAfterFix') THEN
    UPDATE "Order" SET "City" = '' WHERE "City" IS NULL;
    ALTER TABLE "Order" ALTER COLUMN "City" SET NOT NULL;
    ALTER TABLE "Order" ALTER COLUMN "City" SET DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303122130_SyncModelAfterFix') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260303122130_SyncModelAfterFix', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303123046_AddOrderCreatedBy') THEN
    ALTER TABLE "Order" ADD "CreatedBy" nvarchar(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303123046_AddOrderCreatedBy') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260303123046_AddOrderCreatedBy', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303134419_AddUserRoleAndIsApproved') THEN
    ALTER TABLE "UserModelData" ADD "IsApproved" bit NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303134419_AddUserRoleAndIsApproved') THEN
    ALTER TABLE "UserModelData" ADD "Role" nvarchar(max) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303134419_AddUserRoleAndIsApproved') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260303134419_AddUserRoleAndIsApproved', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260303135540_AddUserIdentety') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260303135540_AddUserIdentety', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304170135_AddOrderTotalsAndBrigadeStatus') THEN
    ALTER TABLE "Order" ADD "BrigadeStatus" nvarchar(50);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304170135_AddOrderTotalsAndBrigadeStatus') THEN
    ALTER TABLE "Order" ADD "TotalArrangementAmount" nvarchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304170135_AddOrderTotalsAndBrigadeStatus') THEN
    ALTER TABLE "Order" ADD "TotalDrillingAmount" nvarchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304170135_AddOrderTotalsAndBrigadeStatus') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260304170135_AddOrderTotalsAndBrigadeStatus', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "ArrangementFirstContribution" int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "ArrangementFirstPayment" int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "ArrangementFirstPaymentDueDate" datetime2;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "ArrangementFourthPayment" int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "ArrangementFourthPaymentDueDate" datetime2;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "ArrangementSecondPayment" int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "ArrangementSecondPaymentDueDate" datetime2;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "ArrangementThirdPayment" int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "ArrangementThirdPaymentDueDate" datetime2;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "DrillingFirstContribution" int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "DrillingFirstPayment" int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "DrillingFirstPaymentDueDate" datetime2;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "DrillingFourthPayment" int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "DrillingFourthPaymentDueDate" datetime2;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "DrillingSecondPayment" int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "DrillingSecondPaymentDueDate" datetime2;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "DrillingThirdPayment" int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "DrillingThirdPaymentDueDate" datetime2;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "IsArrangementInstallment" bit NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    ALTER TABLE "Order" ADD "IsDrillingInstallment" bit NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304173301_AddOrderInstallmentsDueDates') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260304173301_AddOrderInstallmentsDueDates', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304174713_UpdateOrderInstallmentsWithErip') THEN
    ALTER TABLE "Order" ADD "InstallmentEripNumber" nvarchar(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260304174713_UpdateOrderInstallmentsWithErip') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260304174713_UpdateOrderInstallmentsWithErip', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306085405_AddBrigadeCompositionAndOrderSubstitutes') THEN
    ALTER TABLE "BrigadeModelData" ADD "AssistantMaster" nvarchar(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306085405_AddBrigadeCompositionAndOrderSubstitutes') THEN
    ALTER TABLE "BrigadeModelData" ADD "BrigadeComposition" nvarchar(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306085405_AddBrigadeCompositionAndOrderSubstitutes') THEN
    ALTER TABLE "BrigadeModelData" ADD "Driver" nvarchar(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306085405_AddBrigadeCompositionAndOrderSubstitutes') THEN
    ALTER TABLE "Order" ADD "SubstituteArrangementAssistantMaster" nvarchar(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306085405_AddBrigadeCompositionAndOrderSubstitutes') THEN
    ALTER TABLE "Order" ADD "SubstituteArrangementDriver" nvarchar(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306085405_AddBrigadeCompositionAndOrderSubstitutes') THEN
    ALTER TABLE "Order" ADD "SubstituteDrillingAssistantMaster" nvarchar(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306085405_AddBrigadeCompositionAndOrderSubstitutes') THEN
    ALTER TABLE "Order" ADD "SubstituteDrillingDriver" nvarchar(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306085405_AddBrigadeCompositionAndOrderSubstitutes') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260306085405_AddBrigadeCompositionAndOrderSubstitutes', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309065706_AddBrigadeDriverAndAssistant') THEN
    ALTER TABLE "Order" DROP COLUMN "SubstituteArrangementAssistantMaster";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309065706_AddBrigadeDriverAndAssistant') THEN
    ALTER TABLE "Order" DROP COLUMN "SubstituteArrangementDriver";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309065706_AddBrigadeDriverAndAssistant') THEN
    ALTER TABLE "Order" DROP COLUMN "SubstituteDrillingAssistantMaster";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309065706_AddBrigadeDriverAndAssistant') THEN
    ALTER TABLE "Order" DROP COLUMN "SubstituteDrillingDriver";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309065706_AddBrigadeDriverAndAssistant') THEN
    ALTER TABLE "BrigadeModelData" DROP COLUMN "BrigadeComposition";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309065706_AddBrigadeDriverAndAssistant') THEN
    ALTER TABLE "BrigadeModelData" RENAME COLUMN "AssistantMaster" TO "DrillingMasterAssistant";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309065706_AddBrigadeDriverAndAssistant') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260309065706_AddBrigadeDriverAndAssistant', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309070544_AddOrderComments') THEN
    CREATE TABLE "OrderComment" (
        "Id" int NOT NULL,
        "OrderId" int NOT NULL,
        "Text" nvarchar(2000) NOT NULL,
        "CreatedAt" datetime2 NOT NULL DEFAULT (GETDATE()),
        "CreatedBy" nvarchar(255),
        "IsDone" bit NOT NULL,
        "DoneAt" datetime2,
        CONSTRAINT "PK_OrderComment" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_OrderComment_Order_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Order" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309070544_AddOrderComments') THEN
    CREATE INDEX "IX_OrderComment_OrderId" ON "OrderComment" ("OrderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309070544_AddOrderComments') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260309070544_AddOrderComments', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309102647_AddOrderInstallmentPaymentStatus') THEN
    CREATE TABLE "OrderInstallmentPaymentStatus" (
        "Id" int NOT NULL,
        "OrderId" int NOT NULL,
        "SlotKey" nvarchar(50) NOT NULL,
        "IsPaid" bit NOT NULL,
        "PaidAt" datetime2,
        CONSTRAINT "PK_OrderInstallmentPaymentStatus" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309102647_AddOrderInstallmentPaymentStatus') THEN
    CREATE UNIQUE INDEX "IX_OrderInstallmentPaymentStatus_OrderId_SlotKey" ON "OrderInstallmentPaymentStatus" ("OrderId", "SlotKey");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309102647_AddOrderInstallmentPaymentStatus') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260309102647_AddOrderInstallmentPaymentStatus', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309105833_AddNotificationTable') THEN
    CREATE TABLE "Notification" (
        "Id" int NOT NULL,
        "OrderId" int,
        "Message" nvarchar(500) NOT NULL,
        "RecipientEmail" nvarchar(255) NOT NULL,
        "IsRead" bit NOT NULL,
        "CreatedAt" datetime2 NOT NULL DEFAULT (GETDATE()),
        CONSTRAINT "PK_Notification" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309105833_AddNotificationTable') THEN
    CREATE INDEX "IX_Notification_OrderId" ON "Notification" ("OrderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309105833_AddNotificationTable') THEN
    CREATE INDEX "IX_Notification_RecipientEmail_IsRead" ON "Notification" ("RecipientEmail", "IsRead");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260309105833_AddNotificationTable') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260309105833_AddNotificationTable', '9.0.7');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260312072234_AddDeviceTokenTable') THEN
    CREATE TABLE "DeviceToken" (
        "Id" int NOT NULL,
        "UserEmail" nvarchar(255) NOT NULL,
        "Token" nvarchar(2048) NOT NULL,
        "Platform" nvarchar(50) NOT NULL,
        "CreatedAt" datetime2 NOT NULL DEFAULT (GETDATE()),
        "UpdatedAt" datetime2 NOT NULL DEFAULT (GETDATE()),
        CONSTRAINT "PK_DeviceToken" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260312072234_AddDeviceTokenTable') THEN
    CREATE UNIQUE INDEX "IX_DeviceToken_Token" ON "DeviceToken" ("Token");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260312072234_AddDeviceTokenTable') THEN
    CREATE INDEX "IX_DeviceToken_UserEmail_Platform" ON "DeviceToken" ("UserEmail", "Platform");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260312072234_AddDeviceTokenTable') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260312072234_AddDeviceTokenTable', '9.0.7');
    END IF;
END $EF$;
COMMIT;

