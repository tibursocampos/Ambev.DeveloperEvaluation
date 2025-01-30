[Back to README](../README.md)

# Sales Payloads

Below are some sample payloads for testing. The dates are valid (no later than today), and the quantities of the items are within the limit of 20 units.

Notes:
- The dates are in ISO 8601 format (YYYY-MM-DDTHH:mm:ss.sssZ).
- Item quantities vary between 1 and 20.
- Some customers and branches are repeated to simulate real scenarios.

---

## Payload 1
```json
{
  "saleDate": "2023-10-25T10:15:30.000Z",
  "customerId": 123,
  "customerName": "John Doe",
  "branchId": 456,
  "branchName": "Prince Store",
  "items": [
    {
      "productId": 101,
      "productName": "Smartphone LTE",
      "quantity": 2,
      "unitPrice": 200.50
    },
    {
      "productId": 203,
      "productName": "Freezer 500l",
      "quantity": 1,
      "unitPrice": 750.00
    },
    {
      "productId": 233,
      "productName": "Headset",
      "quantity": 5,
      "unitPrice": 15.00
    }
  ]
}
```

## Payload 2
```json
{
  "saleDate": "2023-10-24T14:45:00.000Z",
  "customerId": 789,
  "customerName": "Jane Smith",
  "branchId": 456,
  "branchName": "Prince Store",
  "items": [
    {
      "productId": 554,
      "productName": "Office Chair",
      "quantity": 3,
      "unitPrice": 75.00
    },
    {
      "productId": 101,
      "productName": "Smartphone LTE",
      "quantity": 1,
      "unitPrice": 200.50
    }
  ]
}
```

## Payload 3
```json
{
  "saleDate": "2023-10-23T09:30:00.000Z",
  "customerId": 123,
  "customerName": "John Doe",
  "branchId": 789,
  "branchName": "Downtown Store",
  "items": [
    {
      "productId": 233,
      "productName": "Headset",
      "quantity": 10,
      "unitPrice": 15.00
    },
    {
      "productId": 203,
      "productName": "Freezer 500l",
      "quantity": 2,
      "unitPrice": 750.00
    }
  ]
}
```

## Payload 4
```json
{
  "saleDate": "2023-10-22T11:00:00.000Z",
  "customerId": 789,
  "customerName": "Jane Smith",
  "branchId": 789,
  "branchName": "Downtown Store",
  "items": [
    {
      "productId": 101,
      "productName": "Smartphone LTE",
      "quantity": 3,
      "unitPrice": 200.50
    },
    {
      "productId": 233,
      "productName": "Headset",
      "quantity": 2,
      "unitPrice": 15.00
    }
  ]
}
```

## Payload 5
```json
{
  "saleDate": "2023-10-22T16:20:00.000Z",
  "customerId": 456,
  "customerName": "Alice Johnson",
  "branchId": 456,
  "branchName": "Prince Store",
  "items": [
    {
      "productId": 554,
      "productName": "Office Chair",
      "quantity": 4,
      "unitPrice": 75.00
    },
    {
      "productId": 101,
      "productName": "Smartphone LTE",
      "quantity": 2,
      "unitPrice": 200.50
    },
    {
      "productId": 233,
      "productName": "Headset",
      "quantity": 8,
      "unitPrice": 15.00
    }
  ]
}
```

## Payload 6
```json
{
  "saleDate": "2023-10-21T13:30:00.000Z",
  "customerId": 123,
  "customerName": "John Doe",
  "branchId": 456,
  "branchName": "Prince Store",
  "items": [
    {
      "productId": 101,
      "productName": "Smartphone LTE",
      "quantity": 2,
      "unitPrice": 200.50
    },
    {
      "productId": 203,
      "productName": "Freezer 500l",
      "quantity": 1,
      "unitPrice": 750.00
    },
    {
      "productId": 233,
      "productName": "Headset",
      "quantity": 5,
      "unitPrice": 15.00
    }
  ]
}
```

## Payload 7
```json
{
  "saleDate": "2023-10-21T11:10:00.000Z",
  "customerId": 789,
  "customerName": "Jane Smith",
  "branchId": 789,
  "branchName": "Downtown Store",
  "items": [
    {
      "productId": 203,
      "productName": "Freezer 500l",
      "quantity": 1,
      "unitPrice": 750.00
    },
    {
      "productId": 554,
      "productName": "Office Chair",
      "quantity": 2,
      "unitPrice": 75.00
    }
  ]
}
```

## Payload 8
```json
{
  "saleDate": "2023-10-20T13:55:00.000Z",
  "customerId": 123,
  "customerName": "John Doe",
  "branchId": 456,
  "branchName": "Prince Store",
  "items": [
    {
      "productId": 101,
      "productName": "Smartphone LTE",
      "quantity": 3,
      "unitPrice": 200.50
    },
    {
      "productId": 233,
      "productName": "Headset",
      "quantity": 15,
      "unitPrice": 15.00
    }
  ]
}
```

## Payload 9
```json
{
  "saleDate": "2023-10-19T18:05:00.000Z",
  "customerId": 456,
  "customerName": "Alice Johnson",
  "branchId": 789,
  "branchName": "Downtown Store",
  "items": [
    {
      "productId": 203,
      "productName": "Freezer 500l",
      "quantity": 1,
      "unitPrice": 750.00
    },
    {
      "productId": 554,
      "productName": "Office Chair",
      "quantity": 5,
      "unitPrice": 75.00
    }
  ]
}
```

## Payload 10
```json
{
  "saleDate": "2023-10-18T12:30:00.000Z",
  "customerId": 789,
  "customerName": "Jane Smith",
  "branchId": 456,
  "branchName": "Prince Store",
  "items": [
    {
      "productId": 101,
      "productName": "Smartphone LTE",
      "quantity": 2,
      "unitPrice": 200.50
    },
    {
      "productId": 233,
      "productName": "Headset",
      "quantity": 10,
      "unitPrice": 15.00
    },
    {
      "productId": 554,
      "productName": "Office Chair",
      "quantity": 3,
      "unitPrice": 75.00
    }
  ]
}
```

## Payload 11
```json
{
  "saleDate": "2023-10-17T15:40:00.000Z",
  "customerId": 123,
  "customerName": "John Doe",
  "branchId": 789,
  "branchName": "Downtown Store",
  "items": [
    {
      "productId": 203,
      "productName": "Freezer 500l",
      "quantity": 2,
      "unitPrice": 750.00
    },
    {
      "productId": 554,
      "productName": "Office Chair",
      "quantity": 4,
      "unitPrice": 75.00
    }
  ]
}
```

## Payload 12
```json
{
  "saleDate": "2023-10-17T15:40:00.000Z",
  "customerId": 123,
  "customerName": "John Doe",
  "branchId": 789,
  "branchName": "Downtown Store",
  "items": [
    {
      "productId": 203,
      "productName": "Freezer 500l",
      "quantity": 2,
      "unitPrice": 750.00
    },
    {
      "productId": 554,
      "productName": "Office Chair",
      "quantity": 4,
      "unitPrice": 75.00
    }
  ]
}
```

## Payload 13
```json
{
  "saleDate": "2023-10-16T17:50:00.000Z",
  "customerId": 456,
  "customerName": "Alice Johnson",
  "branchId": 456,
  "branchName": "Prince Store",
  "items": [
    {
      "productId": 101,
      "productName": "Smartphone LTE",
      "quantity": 2,
      "unitPrice": 200.50
    },
    {
      "productId": 233,
      "productName": "Headset",
      "quantity": 10,
      "unitPrice": 15.00
    },
    {
      "productId": 554,
      "productName": "Office Chair",
      "quantity": 3,
      "unitPrice": 75.00
    }
  ]
}
```

## Payload 14
```json
{
  "saleDate": "2023-10-16T09:25:00.000Z",
  "customerId": 456,
  "customerName": "Alice Johnson",
  "branchId": 456,
  "branchName": "Prince Store",
  "items": [
    {
      "productId": 101,
      "productName": "Smartphone LTE",
      "quantity": 1,
      "unitPrice": 200.50
    },
    {
      "productId": 233,
      "productName": "Headset",
      "quantity": 20,
      "unitPrice": 15.00
    }
  ]
}
```

## Payload 15
```json
{
  "saleDate": "2023-10-15T14:30:00.000Z",
  "customerId": 789,
  "customerName": "Jane Smith",
  "branchId": 789,
  "branchName": "Downtown Store",
  "items": [
    {
      "productId": 203,
      "productName": "Freezer 500l",
      "quantity": 1,
      "unitPrice": 750.00
    },
    {
      "productId": 554,
      "productName": "Office Chair",
      "quantity": 2,
      "unitPrice": 75.00
    }
  ]
}
```