# Bafang Protocol

## Messages

Notes:

- All request and response values are hex.
- Some messages contain a checksum represented as `%%` which is just every preceding byte summed up

| Name       | Request          | Response   | Description                                                            | Notes                                                                                           |
|------------|------------------|------------|------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------|
| GetRpm     | `11 20`          | `xx xx %%` | ***[uint16]** xx xx*: RPM                                              | For some reason the first call does not return anything and one should not wait for a response. |
| SetPas     | `16 0B xx %%`    |            | ***[uint8]** xx*: PAS Level, see the [PAS section](#PAS) for more info |                                                                                                 |
| GetCurrent | `11 0A`          | `xx %%`    | ***[uint8]** xx*: requested current in A divided by 2                  |                                                                                                 |
| GetBattery | `11 11`          | `xx %%`    | ***[uint8]** xx*: Battery in %                                         |                                                                                                 |
| SetLights? | `16 1A xx`       |            | ***[uint8]** xx*: `F0` - off; `F1` - on                                |                                                                                                 |
| SetMaxRpm? | `16 1F xx xx %%` |            | ***[uint16]** xx xx*: RPM                                              |                                                                                                 |
| GetError?  | `11 08`          | `01`       | ***[uint8]** xx*: `01` - OK                                            |                                                                                                 |
| ?          | `11 22 %%`       |            |                                                                        |                                                                                                 |
| ?          | `11 F0`          |            |                                                                        |                                                                                                 |

### PAS

| Value (hex) | Level (max 3) | Level (max 5) | Level (max 9) |
|-------------|---------------|---------------|---------------|
| `00`        | 0             | 0             | 0             |
| `01`        | 1             | 1             | 1             |
| `0B`        |               | ?             | 2             |
| `0C`        |               | 2?            | 3             |
| `0D`        |               | ?             | 4             |
| `02`        | 2             | 3?            | 5             |
| `15`        |               | ?             | 6             |
| `16`        |               | 4?            | 7             |
| `17`        |               | ?             | 8             |
| `03`        | 3             | 5             | 9             |
| `06`        | ?             | ?             | ?             |
