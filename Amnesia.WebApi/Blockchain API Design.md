# API Design

## Node to Node

### Get specific block

```http
GET /blocks/{hash}
```

```json
{
    "hash": "fba4f4530824dbc5a465cf0eddf7198d91ddec60b6dd1df9cb97accf930c7af0",
    "previous": "edd0679ac620f2a343742eeccb206e267b788c49dfc76c34bd43bdd47d9da9ae",
    "content": "434728a410a78f56fc1b5899c3593436e61ab0c731e9072d95e96db290205e53",
    "nonce": 42
}
```

### Get graph

```http
GET /blocks?depth=5
```

```json
[
    "fba4f4530824dbc5a465cf0eddf7198d91ddec60b6dd1df9cb97accf930c7af0",
    "edd0679ac620f2a343742eeccb206e267b788c49dfc76c34bd43bdd47d9da9ae",
    "434728a410a78f56fc1b5899c3593436e61ab0c731e9072d95e96db290205e53",
]
```

### Inform of block existence

```http
POST /blocks?id={peerId}

"fba4f4530824dbc5a465cf0eddf7198d91ddec60b6dd1df9cb97accf930c7af0"
```

### Get content

```http
GET /blocks/{blockHash}/content
```

```json
{
    "hash": "434728a410a78f56fc1b5899c3593436e61ab0c731e9072d95e96db290205e53",
    "definitions": [
        "3885e3d6ea213501ab07ed2a7b82cc4a719e2190a31df6c4d1e3af407c7c03c2",
        "d23d46f43123b0182ccc4fbc9238cdbbde15d304ce325f4e1ac0eb67edaab853"
    ],
    "mutations": [
        "c35f58bf47176af5b16cabf95945b0d97b75e632b6ceca1d532cd71540e4abac"
    ]
}
```

De blockHash in de url is de hash van het block. De hash property in de response is de hash van de content.

### Get definition

```http
GET /definitions/{hash}
```

```json
{
    "hash": "3885e3d6ea213501ab07ed2a7b82cc4a719e2190a31df6c4d1e3af407c7c03c2",
    "header": {
        "datahash":"5aa03f96c77536579166fba147929626cc3a97960e994057a9d80271a736d10f",
        "meta": {
            "deletable": true
        },
        "previousDefinition": "d23d46f43123b0182ccc4fbc9238cdbbde15d304ce325f4e1ac0eb67edaab853",
        "signature": "38cdbbde15d3"
    },
    "data": {
        "hash": "5aa03f96c77536579166fba147929626cc3a97960e994057a9d80271a736d10f",
        "previousDefinition": "d23d46f43123b0182ccc4fbc9238cdbbde15d304ce325f4e1ac0eb67edaab853",
        "signature": "123b0182cc"
    }
}
```

### Get definition data

```http
GET /definitions/{hash}/data
```

```
Hallo dit is wat data
```

## Client to Node

### Send definition

```http
POST /definitions

{
    "hash": "3885e3d6ea213501ab07ed2a7b82cc4a719e2190a31df6c4d1e3af407c7c03c2",
    "header": {
        "datahash":"5aa03f96c77536579166fba147929626cc3a97960e994057a9d80271a736d10f",
        "meta": {
            "deletable": true
        },
        "previousDefinition": "d23d46f43123b0182ccc4fbc9238cdbbde15d304ce325f4e1ac0eb67edaab853",
        "signature": "38cdbbde15d3"
    },
    "data": {
        "hash": "5aa03f96c77536579166fba147929626cc3a97960e994057a9d80271a736d10f",
        "previousDefinition": "d23d46f43123b0182ccc4fbc9238cdbbde15d304ce325f4e1ac0eb67edaab853",
        "signature": "123b0182cc",
        "blob":"SGFsbG8gZGl0IGlzIHdhdCBkYXRhLCBzdXBlciBsZXVrIGFsbGVtYWFsLCBlYXN0ZXIgZWdnCg=="
    },
}
```

### Get latest definitions

```http
GET /keys/{publicKey}/definitions?limit=5
```

```json
[
    "3885e3d6ea213501ab07ed2a7b82cc4a719e2190a31df6c4d1e3af407c7c03c2",
    "d23d46f43123b0182ccc4fbc9238cdbbde15d304ce325f4e1ac0eb67edaab853"
]
```

