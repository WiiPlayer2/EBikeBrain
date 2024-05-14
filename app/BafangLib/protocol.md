# Bafang Protocol

## Messages

Notes:

- All request and response values are hex.
- Some messages contain a checksum represented as `%%` which is just every preceding byte summed up

| Name       | Request          | Response   | Description                                                            |
|------------|------------------|------------|------------------------------------------------------------------------|
| GetRpm     | `11 20`          | `xx xx %%` | ***[uint16]** xx xx*: RPM                                              |
| SetPas     | `16 0B xx %%`    |            | ***[uint8]** xx*: PAS Level, see the [PAS section](#PAS) for more info | 
| GetCurrent | `11 0A`          | `xx %%`    | ***[uint8]** xx*: requested current in A divided by 2                  |
| GetBattery | `11 11`          | `xx %%`    | ***[uint8]** xx*: Battery in %                                         |
| ?          | `16 1A F0`       |            |                                                                        |
| ?          | `16 1F 00 BA %%` |            |
| ?          | `11 08`          | `01`       |                                                                        |
| ?          | `11 22 %%`       |            |

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
