## Part I
Setting Up Paypal Account
The process involved in setting up a paypal are described as follows
Creating an account on Paypal's website with personal information such as email address and password.
After creating an account and verifying email address, the user has the flexibility to switch to a login to paypal developer account and can switch to the sandbox environment mode from there.
By default, there is an application created on paypal with its credentials i.e. client id and secret. There is also the flexibility of creating a new app with a different credential.
Also by default, there are two sandbox testing accounts (personal and business) created to pay and receive dummy payments from your application.
Paypal payment was therefore integrated into the UoH catering system software using Paypal Javascript SDK to display paypal payment buttons in combination with Paypal REST APIs on the local server.


The credentials testing process involves generating credentials and these credentials (client id and secret key) are verified to ensure their validity by checking the format of the credentials and also ensuring the user possesses appropriate permissions. After the credentials have been verified, the user can make test payments using the paypal sandbox environment to ensure the integration is working perfectly.
After integration and testing of the payment services, the developer can deploy the software to production for Live transactions. In this case, the sandbox account credentials are swapped to Live credentials.
Paypal also continues to monitor API credentials to ensure the validity and its correct usage by analyzing transaction information.

In this project, there are three(3) paypal REST API endpoints that were consumed using C#/.NET and they are as follows:
Authentication (https://api-m.sandbox.paypal.com/v1/oauth2/token): This endpoint was consumed using .NET HttpClient. Which was set up to use the Basic authentication scheme. In this instance, the Client Id and Secret Key were used to set up the authorization request header. These credentials (Client Id and Secret Key) are therefore encoded into a base64-encoded string. The endpoint also takes a query parameter “grant_type=client_credentials” as “x-ww-form-urlencoded”.

![](/UoHCateringServices/wwwroot/assets/1.png)


The format of response from endpoint request for access token for authorization is json as shown below

![](/UoHCateringServices/wwwroot/assets/2.png)



Create order (https://api-m.sandbox.paypal.com/v2/checkout/orders) : This endpoint is responsible for creating order (payment) for a specific order from the catering services application when an order is placed. The http request header for the create order endpoint takes an access_token as the authorization header, content-type of “application/json” and the request body of this endpoint is of JSON format type.

The request body for the API endpoint is of type json which is key/value pair. It takes the “intent” of the transaction, “purchase_units” which consists of the “currency_code” and “value” i.e. the amount. As shown below.



![](/UoHCateringServices/wwwroot/assets/3.png)


The screenshot below shows the request header information and how the http request is sent to the create order endpoint.

![](/UoHCateringServices/wwwroot/assets/4.png)


The response from the capture order endpoint is also in the format of json and the endpoint also returns the HTTP status code 201 as the order id created. Attached below is the response from that endpoint.

![](/UoHCateringServices/wwwroot/assets/5.png)


Capture Order (https://api-m.sandbox.paypal.com/v2/checkout/orders/{id}/capture) : This endpoint captures the order and it takes the order id as the path parameter and also takes “access_token” or paypal Client Id and Secret Key as the authorization on the request header.

![](/UoHCateringServices/wwwroot/assets/6.png)


The response format from the endpoint is of type json and it returns the HTTP status code
201. The returned response includes the order id, transaction status and the payer details.

![](/UoHCateringServices/wwwroot/assets/7.png)




## Paypal Protocol in processing payment
In the context of the built artifact, There is a 3-layered process between the vendor, buyer and paypal and this protocol involves the exchange of data between them. The initial protocol is the protocol between the buyer and the vendor and this involves the buyer sending a request to the vendor’s website server to purchase an item and the vendor’s website displays a payment button which redirects the buyer to the third-party payment handler integrated (in this case, Paypal) for the buyer to make payment.

The subsequent protocol is between the vendor and Paypal whereby the vendor sends a secure HTTPS request to paypal to initiate the payment and this request body includes information about the transaction such as amount, currency code e.t.c. This payment is processed and the status of the payment is sent to the client or server of the vendor’s application.

The other protocol between the buyer and paypal involves the point buyer being redirected to the paypal payment page to enter payment information or card details to complete the payment and the payment status is returned to the vendor’s website.

![](/UoHCateringServices/wwwroot/assets/8.png)

The Paypal Event processes between the buyer, vendor and paypal is explained in the diagram above.


# Weakness of the built paypal integration system

Considering the artifact implemented integrating paypal third-party payment gateway and exchange protocols described above, there are some weaknesses identified in the integration and these include;
Exchange of plain text as payload over a network call such as HTTP request.This payload data can be intercepted and manipulated by hackers during a network call request.
Another weakness identified is the exposure of Client Id on the client-side of the application to render the paypal button used by Javascript SDK. This could be a lead for hackers to use the client id for malicious intent.
Another key weakness pertaining to the flow of payment gateway implementation is the lack of tracking of failed payment/transactions. Integration of Paypal API involves sending requests and receiving responses over the network, and failure or downtime can happen during this network call, so this implementation lacks the flow to cater for transactions that were successful but payers/customers didn’t get

value for. This implementation to track failed payments is called webhook. This current artifact doesn’t include this.
Sending request parameters as query parameters in plain text without encryption is another weakness in the system as this can be observed in the “Generate Access Token” endpoint. This endpoint takes the query parameter “grant_type=client_credentials” in its request.
This can also be further observed on the API endpoint that captures order, this endpoint takes the order id as request path parameter on the URL. The order Id in this case serves as a payment reference and its being exposed on the request path


## Part II

Authentication Encryption: This can be described as a cryptographic technique that consists of encryption and authentication while promoting Confidentiality, Integrity and Authenticity (C.I.A) of exchanged data. This concept ensures that API resources/endpoints are protected from requests that are unauthorized. AES encryption algorithm can be used to achieve this. As shown below is the symmetric encryption algorithm for encrypting password during registration before it is saved to the database.

![](/UoHCateringServices/wwwroot/assets/9.png)


When a registered user wants to log in, the provided password is encrypted and this encrypted text is compared to what is saved on the database before authenticating the user.

Generic Composition Methods of Authenticated Encryptions (AE) can be referred to as a combination of encryption and message authentication code (MAC) which is a value obtained by applying cryptographic hash function on a secret key to enhance confidentiality, integrity, and authenticity of data access. There are about three different methods of combining encryption and MAC and these include Encrypt-and-MAC, MAC-then-Encrypt, and Encrypt-then-MAC.

Encrypt-and-MAC: This technique involves encrypting the plain text with an encryption algorithm and the MAC is generated from the encrypted data. In this type of technique, if a request is sent to a resource, the resource which receives the request first validates the message authentication code (MAC) before decrypting the received data.
Mac-then-Encrypt: This type of technique involves the plain text being hashed with HMAC which is a MAC algorithm. The resulting MAC is then encrypted with an AES symmetric encryption algorithm alongside the data which are both sent as a request.
Encrypt-then-MAC: This type of technique, the plain data is encrypted first with any symmetric encryption algorithm then the MAC is computed from the encrypted message. The MAC is then encrypted with a symmetric encryption algorithm and this encrypted message and the MAC that was encrypted are therefore sent to the endpoint. The endpoint decrypts the MAC first then validates it by using the decrypted message for the decryption if valid.




Single-Pass Authenticated Encryption is best described as an efficient and lightweight authentication encryption type that encrypt and authenticate data in a single pass i.e. data are only processed once which results in cipher text and authentication tag generated at the same time. Due to the lightweight feature of single-pass AE, it makes it suitable for softwares where fast authentication and encryption is needed.
