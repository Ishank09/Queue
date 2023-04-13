Implementation 1 - Way 1:
This is the way the program has implemented Queue. In this approach for each queue instance, a file with unique name will be created which will be used to store the queue instance data. In this the in memory (max size) will be retained, since in memory operations are quicker than on disk operations.
# Assumptions and Limitations
1. Single queue instance not thread safe. ~ Since file operations are required for queue basic operations, parallel computations will lead to simultaneous file operations will can lead to data loss or resource already been used error.

2. The Queue class is not Serializable ~ As the OnDisk data is stored in file rather than a object, default serialization won't work in this case. A seperate manual serialization mthod is required to make it serializable and deserializable.

3. Since disk computations should be used, the time complexity of reading and writing contents in disk can't be removed/optimized.

4. The files will be generated by program, a file for each instance.~ currently handled by automatic garbege collector (deleting file when object is removed frommemory, garbed collector can be called anytime)

5. The Dequeue and peek will return InvalidOperationException when they're performed on a empty queue.

6. The Queue can only retain data of User-defined objects ~ During in memory computations the memory and the references of the Queue element is conserved, as its stored in from-scratch-built linked list. But since On disk elements should be stored in file, the only way to conserve the refenreces of the user defined ibjects we will have to sotre momory of those references and during time of enqueue and dequeue we will have to get the data stored in that location. But since handling memory is not advisable since it can leads to many memory leaks, I have not implemented this appriach. rather than that I am storing data as serialized JToken-JArray, thus data is preserved but not the memory/reference.

7. In memory size will be maintained. for every dequeue operation when in memory is full, a element will be dequeued from in memory, and a element (top) from the disk will be enqueued to inmemory disk. Elements in the disk are not remove when dequeued but only the head index changes to reduce the removing computations (removing element, writing back to disk).

8. In memory size cant be 0 or less than that. It should be 

## Flow/Architecture

1. Queue instantiated with max in-memory size. ~ a new file is generated having a unique name.

2. Enqueue Operation performed (Checking the inMemory size and on the basis of it choosing inmemory/ondish operation)

3. Reading file data (for the particular instance) as JArray.

4. Converting/parsing the Queue element as JToken

5. From the Acquired JArray, we will append the JToken.

6. Writing JArray back to the file


## Design Choices made
Seperate methods are used for in-memory and on-disk storage.

In-Memory Choices:

A from-scratch linkedin like structure is implemented. A NodeWrapper class is used to store data (element of queue) and a next pointer which is pointing to next enqueued element.

This approach was taken over arrary initialization because, array comes with size limitations. Array is of fixed size, and thus when a large in-memory size is provided and only a few enqueue operations are performed or in-memory is emoty to various dequeue operations, then the array will keep that storage/space with it as its of fixed size. Although we might not need the whole memory, the array will keep it occupied. This might cause issue when having lots of instances with large in-memory size. Toavoid this, from-scratch linked list was implemented. Thus it will occupy only the required space. also on dequeue operation, the shifting of elements will take O(n) time.


On-Disk Choices:

A json file is used to store all the data of a instance of the queue. (seperate unique files for each instance of the queue).~ Same file could have been used, with HashTable (Way 2 shown bellow) but this approach will add the overhead time of serializarion and deserialization. Also multiple queue instances can't be created together as they're using/sharing same file this will lead to resource conflict or Data loss. This approach will also limit the number of instances created at a time, too storing all instances in same file can lead to early memory full, and it can lead to increase in time complexity while reading and writing contents of file if a proper data removal is not implemented when object is deleted/removed from memory.

JSon ~ Json object/JArray is used to store indivisual queue instance data. As collections should not be used in this project, using json seems to be the best way to store complex objects. including user defined Queue data, also its easy to work on files with these. And using a json file with this will allow us to use JArray and other featurs.

If not JArray, we could have used size and data as defined way to seperate distinct elements.
For example. 5 "abcde" 2 "ad"  this can be use to store data of a instance of a queue. in which we can get the element (string serialized) by getting the size in of it in start.

Json string serialization is done because if used byte serialization then there are varios data types which are not byte serializable ans our queue will not work for them, example user
defined objects, DataTables etc.

Objects: Json opertions instance are taken as singleton as they can be shared between different objects, but the other classes are used as transient or scoped as per the usecases.

Interfaces Directory: Used to store all interfaces

Services Directory: Used to store all the services.helper functions for queue

Constants Directory: Used to store program constants in a single place, appsettings.json can be used for this. The contants are retrived from this class.

Models Directory: used to store models. program data wrapper.



## External Resources used for help
1. stackoverflow - About Which exception to raise when (what to raise when no element in the queue and dequeue operation is performed), file apis in c#.
2. Microsoft Documentation ~ Get general overview about modules/files/functions like JToken, JObject operations - serializations and deserializations operations. 

## Amount of time took
It took around 10-15 hours to implement everything.






-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------





Way 2 - Not implemented:
This is another way of approaching problem, and in this a single file is used for storing all the instance. This way have many drawbacks. Firstly for storing different instance and retrive them in O(1) time we will need to implement a hashmap. with key as the unique instance id (a increment for new instance) and value as the data of that queue. this approach will require a lots of serialization and deserialization. File -> String -> HashMap -> String -> JArray -> JToken -> Queue Data. This way to access the queue data is require various objects mappings and covwerting which will be more time consuming. Also for working on a instance, in this approach we will have to pull the whole file data containing all instance data. With time this will increase computations in reading and writing the files. 

# Assumptions and Limitations ~ Option 2 unoptimized

1. The implemented Queue is not thread safe and might not work with parallel computations. ~ A single File is used for storing different instances of Queue, For each enqueue/dequeue, the file is being locked so that there is no data loss. But the Count, MaxInMemory, OnDiskCount, and other variables are not thread safe, and the in memory queue is using from-scratch-buld linkedList which is also not thread safe. Further work will be needed to make it thread safe.

2. The Queue class is not Serializable ~ As the OnDisk data is stored in file rather than a object, default serialization won't work in this case. A seperate manual serialization mthod is required to make it serializable and deserializable.

3. The Queue can have atmax 100000 instances. This can be controlled by changing _maxInstances in QueueConstants.cs. ~ Since we're not allowed to use C# Collections, I have used array for storing different instances of queue, and an array has siez litations due to which a defalt sizis provide for nuber of instances.

4. The Queue can only retain data of User-defined objects ~ During in memory computations the memory and the references of the Queue element is conserved, as its stored in from-scratch-built linked list. But since On disk elements should be stored in file, the only way to conserve the refenreces of the user defined ibjects we will have to sotre momory of those references and during time of enqueue and dequeue we will have to get the data stored in that location. But since handling memory is not advisable since it can leads to many memory leaks, I have not implemented this appriach. rather than that I am storing data as serialized string, thus data is preserved but not the memory/reference.

5. The Dequeue and Enqueue will return InvalidOperationException when they're performed on a empty queue.

6. The Queue instances has data limitations ~ Since the Queue elements can be stored in disk/file, having too many instances with too much data can lead to huge file. This can affect the operations. But this can be avoided by having a batch wise priocessing. When a file reaches a particular size we can stop adding instances from that file and make a new file for further operations. Although a mechanisms will also be required for chekcing which instance is present in which file.

7. Batch Enqueue and Dequeue operations can be implemented ~ A batch system can be implemented, thus rather than enqueuing each element in the disk/file we can wait for a buffer/batch to fill up (example 5 enqueue or a combination of 5 enqueue or dequeue operations) and all of the operations in the batch or buffer can be implemented together.

## Flow/Architecture ~ Option 2 unoptimized
In-Memory Enqueue Flow 
1. Queue instantiated with max in-memory size.

2. Enqueue Operation performed

3. Getting file data (file.json) as HashTable. (In file Serialized Hashtable is stored, with key as queue id and value as queue data (json serialized))

4. From the returned HashTable, get the Queue data from queue id key ( queue id is unique to a queue instance) as string and deserialize as JArray. (queue data is stored as JArray serealized string)

5. From the Acquired HashTable, we will add the JToken Enqueued item. (queue element will be converted to JToken) Parse.

6. Now the hashtable will be updated with the new JArray (seriualized), and put this serialized HashTable into the file.

In-Memory Dequeue and Peek Flow
It will be similiar as Enqueue, rather than adding a element we will remove the first element from the JArray and return the first element for Dequeue operation. For peek operation, just returning the first element.
