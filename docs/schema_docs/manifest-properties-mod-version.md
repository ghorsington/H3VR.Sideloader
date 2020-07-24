# Mod version Schema

```txt
https://coder.horse/h3vr/manifest.schema.json#/properties/version
```

Version of the mod


| Abstract            | Extensible | Status         | Identifiable            | Custom Properties | Additional Properties | Access Restrictions | Defined In                                                               |
| :------------------ | ---------- | -------------- | ----------------------- | :---------------- | --------------------- | ------------------- | ------------------------------------------------------------------------ |
| Can be instantiated | No         | Unknown status | Unknown identifiability | Forbidden         | Allowed               | none                | [manifest.schema.json\*](../manifest.schema.json "open original schema") |

## version Type

`string` ([Mod version](manifest-properties-mod-version.md))

## version Constraints

**pattern**: the string must match the following regular expression: 

```regexp
\d+\.\d+(\.\d+)?
```

[try pattern](https://regexr.com/?expression=%5Cd%2B%5C.%5Cd%2B(%5C.%5Cd%2B)%3F "try regular expression with regexr.com")

## version Examples

```json
"1.0.0"
```
