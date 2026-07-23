-- ============================================================
-- 008_BlogPostViews.sql
-- Creates the BlogPostViews table for per-month view tracking.
-- BlogEntries.TimesViewed remains as the all-time total.
-- Only the most recent 13 months of view history is seeded and kept.
-- ============================================================

-- Step 1: Create the BlogPostViews table
CREATE TABLE `BlogPostViews` (
    `BlogPostId`   BIGINT      NOT NULL,
    `Year`         INT         NOT NULL,
    `Month`        INT         NOT NULL,
    `TimesViewed`  INT         NOT NULL DEFAULT 1,
    PRIMARY KEY (`BlogPostId`, `Year`, `Month`),
    CONSTRAINT `FK_BlogPostViews_BlogEntries`
        FOREIGN KEY (`BlogPostId`) REFERENCES `BlogEntries` (`Id`)
        ON DELETE CASCADE
);

-- Step 2: Seed historical data — only posts published within the last
-- 13 months. Records older than that will not be carried forward.
-- The cutoff is: (year * 12 + month) >= (current year * 12 + current month) - 12
INSERT INTO `BlogPostViews` (`BlogPostId`, `Year`, `Month`, `TimesViewed`)
SELECT
    `Id`,
    YEAR(`DatePosted`),
    MONTH(`DatePosted`),
    `TimesViewed`
FROM `BlogEntries`
WHERE `TimesViewed` > 0
  AND (YEAR(`DatePosted`) * 12 + MONTH(`DatePosted`))
      >= (YEAR(NOW()) * 12 + MONTH(NOW()) - 12);

-- NOTE: BlogEntries.TimesViewed is intentionally kept as the all-time
-- running total. BlogPostViews provides monthly granularity (13 months)
-- via the domain event listener.
