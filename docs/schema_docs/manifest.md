# The root schema Schema

```txt
https://coder.horse/h3vr/manifest_schema.json
```

The root schema comprises the entire JSON document.


| Abstract            | Extensible | Status         | Identifiable | Custom Properties | Additional Properties | Access Restrictions | Defined In                                                                 |
| :------------------ | ---------- | -------------- | ------------ | :---------------- | --------------------- | ------------------- | -------------------------------------------------------------------------- |
| Can be instantiated | No         | Unknown status | No           | Forbidden         | Forbidden             | none                | [manifest.schema.json](../out/manifest.schema.json "open original schema") |

## The root schema Type

`object` ([The root schema](manifest.md))

## The root schema Default Value

The default value is:

```json
{}
```

## The root schema Examples

```json
{
  "manifestRevision": "1",
  "guid": "horse.coder.asset_test",
  "name": "Carrot Thompson Mesh",
  "version": "1.0.0",
  "description": "My cool mod!",
  "assetMappings": [
    {
      "type": "Mesh",
      "target": "h3vr_data\\streamingassets\\assets_resources_objectids_weaponry_smg\\thompsonm1a1_magazine:magazine_30Round",
      "path": "carrot:assets\\sosig_melee_crowbar.asset"
    },
    {
      "type": "Material",
      "target": "h3vr_data\\streamingassets\\assets_resources_objectids_weaponry_smg\\thompsonm1a1:m1a1_BaseColor",
      "path": "carrot:assets\\testmaterial.mat"
    },
    {
      "type": "Prefab",
      "target": "h3vr_data\\streamingassets\\assets_resources_objectids_weaponry_smg\\p90",
      "path": "carrot:assets\\cube.prefab"
    }
  ]
}
```

# The root schema Properties

| Property                              | Type     | Required | Nullable       | Defined by                                                                                                                                              |
| :------------------------------------ | -------- | -------- | -------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------ |
| [manifestRevision](#manifestRevision) | `string` | Required | cannot be null | [The root schema](manifest-properties-the-manifestrevision-schema.md "https&#x3A;//coder.horse/h3vr/manifest_schema.json#/properties/manifestRevision") |
| [guid](#guid)                         | `string` | Required | cannot be null | [The root schema](manifest-properties-unique-id-of-the-mod.md "https&#x3A;//coder.horse/h3vr/manifest_schema.json#/properties/guid")                    |
| [name](#name)                         | `string` | Required | cannot be null | [The root schema](manifest-properties-name-of-the-mod.md "https&#x3A;//coder.horse/h3vr/manifest_schema.json#/properties/name")                         |
| [version](#version)                   | `string` | Required | cannot be null | [The root schema](manifest-properties-mod-version.md "https&#x3A;//coder.horse/h3vr/manifest_schema.json#/properties/version")                          |
| [description](#description)           | `string` | Optional | cannot be null | [The root schema](manifest-properties-mod-description.md "https&#x3A;//coder.horse/h3vr/manifest_schema.json#/properties/description")                  |
| [assetMappings](#assetMappings)       | `object` | Required | cannot be null | [The root schema](manifest-properties-asset-mappings.md "https&#x3A;//coder.horse/h3vr/manifest_schema.json#/properties/assetMappings")                 |

## manifestRevision

Revision of the manifest format. The value must be always specific!


`manifestRevision`

-   is required
-   Type: `string` ([The manifestRevision schema](manifest-properties-the-manifestrevision-schema.md))
-   cannot be null
-   defined in: [The root schema](manifest-properties-the-manifestrevision-schema.md "https&#x3A;//coder.horse/h3vr/manifest_schema.json#/properties/manifestRevision")

### manifestRevision Type

`string` ([The manifestRevision schema](manifest-properties-the-manifestrevision-schema.md))

### manifestRevision Constraints

**constant**: the value of this property must be equal to:

```json
"1"
```

### manifestRevision Examples

```json
"1"
```

## guid

GUID of the mod. Must be unique and must have no whitespaces. Preferably should follow reverse domain notation.


`guid`

-   is required
-   Type: `string` ([Unique ID of the mod](manifest-properties-unique-id-of-the-mod.md))
-   cannot be null
-   defined in: [The root schema](manifest-properties-unique-id-of-the-mod.md "https&#x3A;//coder.horse/h3vr/manifest_schema.json#/properties/guid")

### guid Type

`string` ([Unique ID of the mod](manifest-properties-unique-id-of-the-mod.md))

### guid Examples

```json
"horse.coder.asset_test"
```

## name

Name of the mod. Doesn't have to be unique, but must have some text.


`name`

-   is required
-   Type: `string` ([Name of the mod](manifest-properties-name-of-the-mod.md))
-   cannot be null
-   defined in: [The root schema](manifest-properties-name-of-the-mod.md "https&#x3A;//coder.horse/h3vr/manifest_schema.json#/properties/name")

### name Type

`string` ([Name of the mod](manifest-properties-name-of-the-mod.md))

### name Examples

```json
"My cool mod!"
```

## version

Version of the mod


`version`

-   is required
-   Type: `string` ([Mod version](manifest-properties-mod-version.md))
-   cannot be null
-   defined in: [The root schema](manifest-properties-mod-version.md "https&#x3A;//coder.horse/h3vr/manifest_schema.json#/properties/version")

### version Type

`string` ([Mod version](manifest-properties-mod-version.md))

### version Constraints

**pattern**: the string must match the following regular expression: 

```regexp
\d+\.\d+(\.\d+)?
```

[try pattern](https://regexr.com/?expression=%5Cd%2B%5C.%5Cd%2B(%5C.%5Cd%2B)%3F "try regular expression with regexr.com")

### version Examples

```json
"1.0.0"
```

## description

Optional description of the mod.


`description`

-   is optional
-   Type: `string` ([Mod description](manifest-properties-mod-description.md))
-   cannot be null
-   defined in: [The root schema](manifest-properties-mod-description.md "https&#x3A;//coder.horse/h3vr/manifest_schema.json#/properties/description")

### description Type

`string` ([Mod description](manifest-properties-mod-description.md))

### description Examples

```json
"My cool mod!"
```

## assetMappings

An array of mapping objects that describe how to map each in-game asset to an asset defined in the mod


`assetMappings`

-   is required
-   Type: `object` ([Asset mappings](manifest-properties-asset-mappings.md))
-   cannot be null
-   defined in: [The root schema](manifest-properties-asset-mappings.md "https&#x3A;//coder.horse/h3vr/manifest_schema.json#/properties/assetMappings")

### assetMappings Type

`object` ([Asset mappings](manifest-properties-asset-mappings.md))

### assetMappings Examples

```json
[
  {
    "type": "Mesh",
    "target": "h3vr_data\\streamingassets\\assets_resources_objectids_weaponry_smg\\thompsonm1a1_magazine:magazine_30Round",
    "path": "carrot:assets\\sosig_melee_crowbar.asset"
  },
  {
    "type": "Material",
    "target": "h3vr_data\\streamingassets\\assets_resources_objectids_weaponry_smg\\thompsonm1a1:m1a1_BaseColor",
    "path": "carrot:assets\\testmaterial.mat"
  }
]
```
