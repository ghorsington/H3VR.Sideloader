# Asset mappings Schema

```txt
https://coder.horse/h3vr/manifest.schema.json#/properties/assetMappings
```

An array of mapping objects that describe how to map each in-game asset to an asset defined in the mod


| Abstract            | Extensible | Status         | Identifiable            | Custom Properties | Additional Properties | Access Restrictions | Defined In                                                                   |
| :------------------ | ---------- | -------------- | ----------------------- | :---------------- | --------------------- | ------------------- | ---------------------------------------------------------------------------- |
| Can be instantiated | No         | Unknown status | Unknown identifiability | Forbidden         | Allowed               | none                | [manifest.schema.json\*](../out/manifest.schema.json "open original schema") |

## assetMappings Type

`object` ([Asset mappings](manifest-properties-asset-mappings.md))

## assetMappings Examples

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
