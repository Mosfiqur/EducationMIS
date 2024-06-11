-- C#
SELECT 
	concat('public const string ', PermissionName, ' = ', '"',PermissionName,'";')
FROM unicefedudb.permissions;
-- Typescript
SELECT 
	concat('public static ', PermissionName, ': string = ', '"',PermissionName,'";')
FROM unicefedudb.permissions;



