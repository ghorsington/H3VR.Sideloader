# Type of the mapping Schema

```txt
#/properties/assetMappings/items#/properties/assetMappings/items/anyOf/0/properties/type
```

The type of the asset mapping that will tell the loader how to handle the provided asset


| Abstract            | Extensible | Status         | Identifiable            | Custom Properties | Additional Properties | Access Restrictions | Defined In                                                               |
| :------------------ | ---------- | -------------- | ----------------------- | :---------------- | --------------------- | ------------------- | ------------------------------------------------------------------------ |
| Can be instantiated | No         | Unknown status | Unknown identifiability | Forbidden         | Allowed               | none                | [manifest.schema.json\*](../manifest.schema.json "open original schema") |

## type Type

unknown ([Type of the mapping](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-type-of-the-mapping.md))

## type Constraints

**enum**: the value of this property must be equal to one of the following values:

| Value        | Explanation |
| :----------- | ----------- |
| `"Mesh"`     |             |
| `"Prefab"`   |             |
| `"Texture"`  |             |
| `"Material"` |             |

## type Examples

```json
"Mesh"
```
