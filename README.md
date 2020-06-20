This application responsibility is to process credit card payments received from the merchant. This payment gateway API has two endpoints one processes the payments whilst the other retrieve the relevant details of the payment. 

##### Endpoints  

This endpoint will process the merchant payment.
A sample input is below:

#### ProcessPayments

**Route**

api/Payment/ProcessPayment - POST endpoint

**Requests**

A sample request below:

```JSON
{
  "merchantId": "87C4DA44-94D4-4DDC-B242-20A5098FE614",
  "amount": 146,
  "currency": "GBP",
  "card": {
    "cardNumber": "4242424242424242",
    "cardExpiryMonth": "06",
    "cardExpiryYear": "2021",
    "cvv": "120",
    "cardHolderName": "test48"
  }
}
```

**Response**

A sample response is below:

```JSON
{
  "result": "Success PaymentSuccessful",
  "errorMessage": null
}
```

 #### GetPaymentTransaction

**Route**

api/Payment/GetPaymentTransaction

**Request**

A sample request is below:

api/Payment/GetPaymentTransaction?id=13

**Response**

```JSON
{
  "currency": "GBP",
  "amount": 146,
  "card": {
    "cardNumber": "XXXX XXXX XXXX 4242 ",
    "cardExpiryMonth": 6,
    "cardExpiryYear": 2022,
    "cvv": null,
    "cardHolderName": "test48"
  },
  "merchantName": "Test Merchant 2",
  "bankReferenceIdentifier": "cf2bcec6-5103-40da-a8c0-2b0b2dc02ba0",
  "status": "Success"
}
```

### Authentication

All requests require an  X-Api-Key header value, which in this case for the purposes of using app I will inform you of the value which is 072413ac-9b28-401f-a0d1-8512d7609dd8. I wouldn't normally do this of course but this is a tech test so certain information has to be given that wouldn't normally be done in a real word scenario. 

### Database

The database will need to be created for the application to be tested locally. I used EF.Core migrations, so you will need to change the conectionString to match your localhost db server. Once that is done use the command "Update-Database" that should run the latest migration. 