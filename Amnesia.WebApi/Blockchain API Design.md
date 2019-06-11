# 	API Design

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
    "dataHash":"5aa03f96c77536579166fba147929626cc3a97960e994057a9d80271a736d10f",
    "isMutation": false,
    "isMutable": true,
    "previousDefinition": "d23d46f43123b0182ccc4fbc9238cdbbde15d304ce325f4e1ac0eb67edaab853",
    "signature": "T+mJbidbQkwgB5oVhtDDZNWiuSySt",
    "key": "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiGAB\n-----END PUBLIC KEY-----\n"
}
```

### Get definition data

```http
GET /definitions/{hash}/data
```

```
{
    "hash": "3885e3d6ea213501ab07ed2a7b82cc4a719e2190a31df6c4d1e3af407c7c03c2",
    "previousDefinition": "d23d46f43123b0182ccc4fbc9238cdbbde15d304ce325f4e1ac0eb67edaab853",
    "signature": "T+mJbidbQkwgB5oVhtDDZNWiuSySt",
    "key": "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiGAB\n-----END PUBLIC KEY-----\n"
}
```

### Get definition blob

```http
GET /definitions/{hash}/data/blob
```

```
Hallo dit is wat data
```

## Client to Node

### Send definition

```http
POST /definitions

{
    "definition": {
        "datahash":"5aa03f96c77536579166fba147929626cc3a97960e994057a9d80271a736d10f",
        "isMutable": true,
        "isMutation": false,
        "previousDefinition": "d23d46f43123b0182ccc4fbc9238cdbbde15d304ce325f4e1ac0eb67edaab853",
        "signature": "T+mJbidbQkwgB5oVhtDDZNWiuSySt",
        "key": "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiGAB\n-----END PUBLIC KEY-----\n"
    },
    "data": {
        "hash": "5aa03f96c77536579166fba147929626cc3a97960e994057a9d80271a736d10f",
        "previousDefinition": "d23d46f43123b0182ccc4fbc9238cdbbde15d304ce325f4e1ac0eb67edaab853",
        "signature": "T+mJbidbQkwgB5oVhtDDZNWiuSySt",
        "key": "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiGAB\n-----END PUBLIC KEY-----\n",
        "blob":"SGFsbG8gZGl0IGlzIHdhdCBkYXRhLCBzdXBlciBsZXVrIGFsbGVtYWFsLCBlYXN0ZXIgZWdnCg=="
    },
}
```

### Get latest definition

```http
POST /definitions/last

{
    "publicKey":"-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiGAB\n-----END PUBLIC KEY-----\n"
}
```

```json
"3885e3d6ea213501ab07ed2a7b82cc4a719e2190a31df6c4d1e3af407c7c03c2"
```

