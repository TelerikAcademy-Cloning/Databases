<!-- section start -->

<!-- attr: {id: 'title', class: 'slide-title', hasScriptWrapper: true} -->
# MongoDB and using MongoDB with .NET
## Welcome to the JSON-stores world

<div class="signature">
    <p class="signature-course">Databases</p>
    <p class="signature-initiative">Telerik Software Academy</p>
    <a href="http://academy.telerik.com" class="signature-link">http://academy.telerik.com</a>
</div>

<!-- section start -->

<!-- attr: {id: 'table-of-contents'} -->
# Table of Contents

*   MongoDB Overview
    *   Installation and drivers
    *   Structure and documents
*   Hosting locally MongoDB
*   DB Viewers
    *   Command-line interface (CLI), UMongo
*   Queries

<!-- section start -->

<!-- attr: {class: 'slide-section'} -->
#   MongoDB Overview
##  Document-based databases

#   MongoDB

*   MongoDB is an open-source document store database
    *   Stores JSON-style objects with dynamic schemas
    *   Support for indices
    *   Has document-based queries
        *   CRUD operations

#   Installing MongoDB

*   Download the latest MongoDB from http://https://mongodb.org/downloads
    *   Installers for all major platforms
*    When installed, MongoDB needs a driver to be usable with each platform:
    *   One for .NET, another for Node.JS, etc...


<!-- attr: {class: 'slide-section'} -->
#   Installing MongoDB
##  [Demo](http://)

#   Running MongoDB locally

*   Once installed, MongoDB can be started
    *   Navigate to the MongoDB install folder in CMD
    *   And run: `$ mongod`

        *   May be necessary to create folder `c:\data\db`  

*   While running, MongoDB can be used with any language and platform
    *   Provided there is a driver for this platform

<!-- attr: {class: 'slide-section'} -->
#   Running MongoDB locally
##  [Demo](http://)

<!-- section start -->

<!-- attr: {class: 'slide-section'} -->
#   Using MongoDB in .NET
##  Packages and stuff

#   Using MongoDB in .NET

*   To use MongoDB from .NET, a MongoDB driver for .NET must be installed
    *   The official is http://docs.mongodb.org/ecosystem/drivers/csharp/

*   To install it, just type in Package Management Console:

    ```
    PM> Install-Package MongoDB.Driver -Version 2.0.1
    ```

#   Using MongoDB in .NET

*   Once installed, you can connect and use MongoDB:

```cs
var connectionString = "mongodb://localhost";
//var connectionString = "URL_IN_THE_WEB";

var client = new MongoClient(connectionString);
var db = client.GetDatabase("books");
```

<!-- attr: {class: 'slide-section'} -->
#   Using MongoDB in .NET
##  [Demo](http://)

<!-- section start -->

#   MongoDB Viewers

*   MongoDB is an open-source DB system
    *   So there are many available viewers
*   Some, but not all are:  
    *   MongoDB CLI
        *   Comes with installation of MongoDB
        *   Execute queries inside the CMD/Terminal
        *   The most commonly used
    *   MongoVUE & UMongo
        *   Provide UI to view, edit are remove DB documents
        *   Kind of sloppy

<!-- attr: {class: 'slide-section'} -->
#   MongoDB Viewers
##  Demo

<!-- section start -->

<!-- attr: {class: 'slide-section'} -->
#   MongoDB Driver APIs: Insert, Read, Update, Remove
##  Working with MongoDB from C#

# MongoDB Driver APIs

- The MongoDB driver provides all necessary functionality for querying the database:
  - All CRUD operations (Create, READ, Update, Delete)
  - Some additional functionalities for easier working with database
    - Like conditional search, delete and update
  - All queries are asynchronous
    - Can be used synchronous as well, but it is not recommended


#   MongoDB Driver APIs: Find

- Getting documents from MongoDB:
  - Use the `FindSync()` and `FindAsync()` methods
    - They expect a delegate as a parameter
  - _Example:_
    - Synchronous:

    ```cs
    var superheroes = superheroesCollection.FindSync((x) => true);
    var flyingSuperheroes = superheroesCollection.FindSync(
      x =>
        x.Powers.Any(
          power =>
            power.Name.ToLower().Contains("flying")));
    ```

    - Asynchronous:
    ```cs
    var superheroes = await superheroesCollection.FindAsync((x) => true);
    var flyingSuperheroes = await superheroesCollection.FindAsync(
      x =>
        x.Powers.Any(
          power =>
            power.Name.ToLower().Contains("flying")));
    ```

<!-- attr: {class: "slide-section", showInPresentation: true} -->
<!-- # Reading from MongoDB -->
##  Demo

#   MongoDB Driver APIs: Insert

- Inserting an object with MongoDB driver can be done either asynchronous or synchronous:
  - Synchronous:

    ```cs
        this.Collection.InsertOne(entity);
    ```

  - Asynchronous (recommended):

    ```cs
    await this.Collection.InsertOneAsync(entity)
    ```

#   MongoDB Driver APIs: Insert

- The MongoDB driver also supports batch insert
  - i.e. add a collection of objects to a collection, all at once, with a single query:
  - Synchronous:

    ```cs
    IEnumerable<Superheroes> superheroes = ...;
        this.Collection.InsertMany(superheroes);
    ```

  - Asynchronous (recommended):

    ```cs
    IEnumerable<Superheroes> superheroes = ...;
    await this.Collection.InsertManyAsync(superheroes)
    ```

<!-- attr: {class: "slide-section", showInPresentation: true} -->
<!-- # Inserting documents with MongoDB driver -->
##  Demo

# Deleting with MongoDB Driver

- The MongoDB driver supports delete as well:

    ```cs
    //  Synchronous
    collection.DeleteOne(sh => deleteCondition(sh));
    collection.DeleteMany(sh => deleteCondition(sh));
    //  Asynchronous
    await collection.DeleteOneAsync(sh => deleteCondition(sh));
    await collection.DeleteManyAsync(sh => deleteCondition(sh));
    ```

<!-- attr: {class: "slide-section", showInPresentation: true} -->
<!-- # Deleting documents with MongoDB driver -->
##  Demo

# Updating with MongoDB Driver

- The update is stranger than the other queries:
  1.  Create a find filter

    ```cs
    var filter = x => x.SecretIdentity == "Dick Grayson";
    ```

  2.  Create an update definition

    ```cs
    var updateDefinition = new UpdateDefinitionBuilder<Superhero>()
      .Set("Secret", "Nightwing")
    ```

  3.  Update the values in the database

    ```cs
    collection.UpdateOne(filter, updateDefinition);
    await collection.UpdateOneAsync(filter, updateDefinition);
    await collection.UpdateManyAsync(filter, updateDefinition);
    ```

<!-- attr: {class: "slide-section", showInPresentation: true} -->
<!-- # Updating documents with MongoDB driver -->
##  Demo

<!-- section start -->

<!-- attr: {id: 'questions', class: 'slide-questions', showInPresentation: true} -->
# MongoDB and MongoDB in .NET
## Questions
