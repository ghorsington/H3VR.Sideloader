# An asset mapping Schema

```txt
#/properties/assetMappings/items#/properties/assetMappings/items/anyOf/0
```

A single mapping of an in-game asset to the asset defined in the mod


| Abstract            | Extensible | Status         | Identifiable | Custom Properties | Additional Properties | Access Restrictions | Defined In                                                               |
| :------------------ | ---------- | -------------- | ------------ | :---------------- | --------------------- | ------------------- | ------------------------------------------------------------------------ |
| Can be instantiated | No         | Unknown status | No           | Forbidden         | Allowed               | none                | [manifest.schema.json\*](../manifest.schema.json "open original schema") |

## 0 Type

unknown ([An asset mapping](manifest-properties-asset-mappings-items-anyof-an-asset-mapping.md))

## 0 Examples

```json
{
  "type": "Mesh",
  "target": "h3vr_data\\streamingassets\\assets_resources_objectids_weaponry_smg\\thompsonm1a1_magazine:magazine_30Round",
  "path": "carrot:assets\\sosig_melee_crowbar.asset"
}
```

# An asset mapping Properties

| Property              | Type          | Required | Nullable       | Defined by                                                                                                                                                                                                                                                           |
| :-------------------- | ------------- | -------- | -------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [type](#type)         | Not specified | Required | cannot be null | [The root schema](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-type-of-the-mapping.md "\#/properties/assetMappings/items#/properties/assetMappings/items/anyOf/0/properties/type")                                                     |
| [target](#target)     | `string`      | Required | cannot be null | [The root schema](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-target-string-that-describes-how-to-find-the-in-game-asset-to-replace.md "\#/properties/assetMappings/items#/properties/assetMappings/items/anyOf/0/properties/target") |
| [path](#path)         | `string`      | Required | cannot be null | [The root schema](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-path-to-replacement-asset.md "\#/properties/assetMappings/items#/properties/assetMappings/items/anyOf/0/properties/path")                                               |
| Additional Properties | Any           | Optional | can be null    |                                                                                                                                                                                                                                                                      |

## type

The type of the asset mapping that will tell the loader how to handle the provided asset


`type`

-   is required
-   Type: unknown ([Type of the mapping](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-type-of-the-mapping.md))
-   cannot be null
-   defined in: [The root schema](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-type-of-the-mapping.md "\#/properties/assetMappings/items#/properties/assetMappings/items/anyOf/0/properties/type")

### type Type

unknown ([Type of the mapping](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-type-of-the-mapping.md))

### type Constraints

**enum**: the value of this property must be equal to one of the following values:

| Value        | Explanation |
| :----------- | ----------- |
| `"Mesh"`     |             |
| `"Prefab"`   |             |
| `"Texture"`  |             |
| `"Material"` |             |

### type Examples

```json
"Mesh"
```

## target

Target string is a collection of substrings separated by `:`. The strings define a path to the actual object to replace. The contents depend on the target value, so consult the docs for this property's value.


`target`

-   is required
-   Type: `string` ([Target string that describes how to find the in-game asset to replace](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-target-string-that-describes-how-to-find-the-in-game-asset-to-replace.md))
-   cannot be null
-   defined in: [The root schema](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-target-string-that-describes-how-to-find-the-in-game-asset-to-replace.md "\#/properties/assetMappings/items#/properties/assetMappings/items/anyOf/0/properties/target")

### target Type

`string` ([Target string that describes how to find the in-game asset to replace](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-target-string-that-describes-how-to-find-the-in-game-asset-to-replace.md))

### target Examples

```json
"h3vr_data\\streamingassets\\assets_resources_objectids_weaponry_smg\\thompsonm1a1_magazine:magazine_30Round"
```

## path

Path to an asset inside the mod (relative to mod's directory) that will be used to replace the original assset. Consult the docs for more info.


`path`

-   is required
-   Type: `string` ([Path to replacement asset](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-path-to-replacement-asset.md))
-   cannot be null
-   defined in: [The root schema](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-path-to-replacement-asset.md "\#/properties/assetMappings/items#/properties/assetMappings/items/anyOf/0/properties/path")

### path Type

`string` ([Path to replacement asset](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-path-to-replacement-asset.md))

### path Examples

```json
"carrot:assets\\sosig_melee_crowbar.asset"
```

## Additional Properties

Additional properties are allowed and do not have to follow a specific schema
