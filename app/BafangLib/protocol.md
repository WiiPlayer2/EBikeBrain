# Bafang Protocol

## Messages

Notes:

- All request and response values are hex.
- All responses contain a checksum represented as `%%`

| Name   | Request | Response | Description         |
|--------|---------|----------|---------------------|
| GetRpm | 11 20   | xx xx %% | [uint16] xx xx: RPM |