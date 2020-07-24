# Target string that describes how to find the in-game asset to replace Schema

```txt
#/properties/assetMappings/items#/properties/assetMappings/items/anyOf/0/properties/target
```

Target string is a collection of substrings separated by `:`. The strings define a path to the actual object to replace. The contents depend on the target value, so consult the docs for this property's value.


| Abstract            | Extensible | Status         | Identifiable            | Custom Properties | Additional Properties | Access Restrictions | Defined In                                                               |
| :------------------ | ---------- | -------------- | ----------------------- | :---------------- | --------------------- | ------------------- | ------------------------------------------------------------------------ |
| Can be instantiated | No         | Unknown status | Unknown identifiability | Forbidden         | Allowed               | none                | [manifest.schema.json\*](../manifest.schema.json "open original schema") |

## target Type

`string` ([Target string that describes how to find the in-game asset to replace](manifest-properties-asset-mappings-items-anyof-an-asset-mapping-properties-target-string-that-describes-how-to-find-the-in-game-asset-to-replace.md))

## target Examples

```json
"h3vr_data\\streamingassets\\assets_resources_objectids_weaponry_smg\\thompsonm1a1_magazine:magazine_30Round"
```
