![Realm](https://github.com/adamfisher/Realm.KeyValueStorage/blob/master/graphics/realm-logo.png?raw=true)

Realm is a mobile database that runs directly inside phones, tablets or wearables.

This library extends the Realm mobile database to provide convenient key/value based storage. This is useful in situations where you have one-off or single pieces of data like:

- user information (name, last login timestamp, pin code, etc.)
- settings/preferences
- temporary data

## Getting Started

This library is a lightweight wrapper around Realm so be sure to check out the official [Realm .NET repository](https://github.com/realm/realm-dotnet) for documentation and examples on advanced querying and usage of Realm.

```csharp
// Get a KeyValueRealm instance
var realm = KeyValueRealm.GetInstance();

// Save some values
realm.Set("First Name", "John");
realm.Set("PinCode", 1234);

// Retrieve the pin code strongly typed as an integer
var pinCode = realm.Get<int>("PinCode"); // pinCode == 1234

// Remove the first name
realm.Remove("First Name");
var firstName = realm.Get("firstName"); // firstName == null

// Set a key named "Married" that expires two days from now
realm.Set("Married", true, DateTimeOffset.Now.AddDays(2));
```

### KeyValueRealm

`KeyValueRealm` is a lightweight wrapper around a standard `Realm` object providing convenience methods specifically optimized for working with key/value based storage.

You call `KeyValueRealm.GetInstance()` to retrieve an instance, optionally passing the same types of parameters as you would when calling `Realm.GetInstance()` normally. You have complete control of the underlying Realm. This class just makes it easier to work with keys and values.

### KeyValueItem

A `KeyValueItem` is a Realm object that contains **Key**, **Value**, and **ExpiresOn** fields. This is the main object you interact with and use with a `KeyValueRealm` to save the item. There are convenience methods as shown in the **Getting Started** section above that handle creating and modifying these objects for you.

### Expiring Keys

Keys can be set to expire after a specified time. If a key is queried and it is after it's expiration, it will be removed from the database and `null` will be returned.

## License

Realm Key Value Storage is licensed the same as the Realm project. Realm Key Value Storage is published under the Apache 2.0 license.
