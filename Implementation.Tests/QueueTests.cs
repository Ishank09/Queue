// Copyright (c) Docugami, Inc. All rights reserved.

using System;

namespace TakeHome.Implementation.Tests
{
    using Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Reflection;
    using System.Threading;
    using System.Data;

    [TestClass]
    public class QueueTests
    {
        /// <summary>
        /// test Enqueue, Peek, count
        /// </summary>
        [TestMethod]
        public void TestPeek()
        {
            var queue = new Queue<int>(10);

            queue.Enqueue(1);
            Assert.IsTrue(queue.Peek() == 1);
            Assert.IsTrue(queue.Count == 1);
            Assert.IsTrue(queue.InMemoryCount == 1);
            Assert.IsTrue(queue.OnDiskCount == 0);
        }

        /// <summary>
        /// test Enqueue, Peek on empty Queue
        /// </summary>
        [TestMethod]
        public void TestPeekEmptyQueue()
        {
            try
            {
                var queue = new Queue<int>(10);

                queue.Enqueue(1);
                Assert.IsTrue(queue.Count == 1);
                Assert.IsTrue(queue.InMemoryCount == 1);
                Assert.IsTrue(queue.OnDiskCount == 0);
                queue.Dequeue();
                Assert.IsTrue(queue.Count == 0);
                Assert.IsTrue(queue.InMemoryCount == 0);
                Assert.IsTrue(queue.OnDiskCount == 0);
                queue.Peek();
                Assert.Fail("no exception thrown");

            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException);
            }
        }

        /// <summary>
        /// test Enqueue, Dequeue on Empty Queue
        /// </summary>
        [TestMethod]
        public void TestDequeueEmptyQueue()
        {
            try
            {
                var queue = new Queue<int>(10);

                queue.Enqueue(1);
                queue.Dequeue();
                queue.Dequeue();
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException);
            }

        }

        /// <summary>
        /// Multiple Enqueue, Peek, Count
        /// </summary>
        [TestMethod]
        public void TestEnqueueOnDisk()
        {
            try
            {
                var queue = new Queue<int>(3);

                queue.Enqueue(1);
                queue.Enqueue(2);
                queue.Enqueue(3);
                Assert.IsTrue(queue.Count == 3);
                Assert.IsTrue(queue.InMemoryCount == 3);
                Assert.IsTrue(queue.OnDiskCount == 0);
                queue.Enqueue(4);
                queue.Enqueue(5);
                Assert.IsTrue(queue.Count == 5);
                Assert.IsTrue(queue.InMemoryCount == 3);
                Assert.IsTrue(queue.OnDiskCount == 2);
                queue.Enqueue(6);
                queue.Enqueue(7);
                Assert.IsTrue(queue.Peek() == 1);
                Assert.IsTrue(queue.Count == 7);

            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException);
            }

        }

        /// <summary>
        /// Multiple Enqueue, Peek, Count, Dequeue in memory
        /// </summary>
        [TestMethod]
        public void TestDequeueInMemory1()
        {
            try
            {
                var queue = new Queue<int>(3);

                queue.Enqueue(1);
                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(4);
                queue.Enqueue(5);
                queue.Enqueue(6);
                queue.Enqueue(7);
                Assert.IsTrue(queue.Count == 7);
                Assert.IsTrue(queue.InMemoryCount == 3);
                Assert.IsTrue(queue.OnDiskCount == 4);

                var dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 1);
                Assert.IsTrue(queue.InMemoryCount == 3);
                Assert.IsTrue(queue.OnDiskCount == 3);
                Assert.IsTrue(queue.Peek() == 2);
                Assert.IsTrue(queue.Count == 6);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 2);
                Assert.IsTrue(queue.InMemoryCount == 3);
                Assert.IsTrue(queue.OnDiskCount == 2);
                Assert.IsTrue(queue.Peek() == 3);
                Assert.IsTrue(queue.Count == 5);


                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 3);
                Assert.IsTrue(queue.InMemoryCount == 3);
                Assert.IsTrue(queue.OnDiskCount == 1);
                Assert.IsTrue(queue.Peek() == 4);
                Assert.IsTrue(queue.Count == 4);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 4);
                Assert.IsTrue(queue.InMemoryCount == 3);
                Assert.IsTrue(queue.OnDiskCount == 0);
                Assert.IsTrue(queue.Peek() == 5);
                Assert.IsTrue(queue.Count == 3);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 5);
                Assert.IsTrue(queue.InMemoryCount == 2);
                Assert.IsTrue(queue.OnDiskCount == 0);
                Assert.IsTrue(queue.Peek() == 6);
                Assert.IsTrue(queue.Count == 2);


                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 6);
                Assert.IsTrue(queue.InMemoryCount == 1);
                Assert.IsTrue(queue.OnDiskCount == 0);
                Assert.IsTrue(queue.Peek() == 7);
                Assert.IsTrue(queue.Count == 1);

            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException);
            }

        }

        /// <summary>
        /// Multiple Enqueue, Peek, Count, Dequeue in disk
        /// </summary>
        [TestMethod]
        public void TestDequeueInMemory2()
        {
            try
            {
                var queue = new Queue<int>(3);

                queue.Enqueue(1);
                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(4);
                queue.Enqueue(5);
                queue.Enqueue(6);
                queue.Enqueue(7);
                var dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 1);
                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 2);
                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 3);
                Assert.IsTrue(queue.Peek() == 4);
                Assert.IsTrue(queue.Count == 4);

            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException);
            }

        }

        /// <summary>
        /// Multiple Enqueue, Peek, Count, Dequeue in disk
        /// </summary>
        [TestMethod]
        public void TestDequeueInMemory3()
        {
            try
            {
                var queue = new Queue<int>(1);

                queue.Enqueue(1);
                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(4);
                queue.Enqueue(5);
                queue.Enqueue(6);
                queue.Enqueue(7);
                Assert.IsTrue(queue.Count == 7);
                Assert.IsTrue(queue.InMemoryCount == 1);
                Assert.IsTrue(queue.OnDiskCount == 6);
                var dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 1);
                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 2);
                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 3);
                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 4);
                Assert.IsTrue(queue.Peek() == 5);
                Assert.IsTrue(queue.Count == 3);

            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException);
            }

        }


        /// <summary>
        /// Multiple Enqueue, Multiple Dequeue
        /// </summary>
        [TestMethod]
        public void TestDequeueInMemory4()
        {
            try
            {
                var queue = new Queue<int>(1);

                queue.Enqueue(1);
                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(4);
                queue.Enqueue(5);
                queue.Enqueue(6);
                queue.Enqueue(7);
                var dequeueElement = queue.Dequeue();

                Assert.IsTrue(dequeueElement == 1);
                dequeueElement = queue.Dequeue();

                Assert.IsTrue(dequeueElement == 2);
                dequeueElement = queue.Dequeue();

                Assert.IsTrue(dequeueElement == 3);
                dequeueElement = queue.Dequeue();

                Assert.IsTrue(dequeueElement == 4);
                Assert.IsTrue(queue.Peek() == 5);
                Assert.IsTrue(queue.Count == 3);
                dequeueElement = queue.Dequeue();

                Assert.IsTrue(dequeueElement == 5);
                Assert.IsTrue(queue.Peek() == 6);
                Assert.IsTrue(queue.Count == 2);
                dequeueElement = queue.Dequeue();

                Assert.IsTrue(dequeueElement == 6);
                Assert.IsTrue(queue.Peek() == 7);
                Assert.IsTrue(queue.Count == 1);
                dequeueElement = queue.Dequeue();

                Assert.IsTrue(dequeueElement == 7);
                Assert.IsTrue(queue.Count == 0);
                Assert.ThrowsException<InvalidOperationException>(() => queue.Peek());

                Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());


            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException);
            }

        }

        /// <summary>
        /// Multiple Enqueue, Multiple Dequeue
        /// </summary>
        [TestMethod]
        public void TestQueueFunctionality()
        {
            try
            {
                var queue = new Queue<int>(3);
                Assert.IsTrue(queue.Count == 0);

                queue.Enqueue(1);
                queue.Enqueue(2);
                queue.Enqueue(3);
                Assert.IsTrue(queue.Count == 3);
                Assert.IsTrue(queue.Peek() == 1);

                var dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 1);
                Assert.IsTrue(queue.Count == 2);
                Assert.IsTrue(queue.Peek() == 2);
                queue.Enqueue(4);
                Assert.IsTrue(queue.Count == 3);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 2);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 3);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 4);
                Assert.IsTrue(queue.Count == 0);

                Assert.ThrowsException<InvalidOperationException>(() => queue.Peek());

                Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Multiple Enqueue, Multiple Dequeue, string data
        /// </summary>
        [TestMethod]
        public void TestQueueComplexScenario()
        {
            try
            {
                var queue = new Queue<string>(5);
                queue.Enqueue("A");
                queue.Enqueue("B");
                queue.Enqueue("C");
                queue.Enqueue("D");
                queue.Enqueue("E");
                Assert.IsTrue(queue.Count == 5);

                Assert.IsTrue(queue.Peek() == "A");
                var dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "A");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "B");
                queue.Enqueue("F");
                queue.Enqueue("G");
                Assert.IsTrue(queue.Count == 5);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "C");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "D");
                Assert.IsTrue(queue.Peek() == "E");
                queue.Enqueue("H");
                queue.Enqueue("I");
                queue.Enqueue("J");
                Assert.IsTrue(queue.Count == 6);
                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "E");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "F");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "G");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "H");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "I");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "J");
                Assert.IsTrue(queue.Count == 0);

                Assert.ThrowsException<InvalidOperationException>(() => queue.Peek());
                Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Multiple Enqueue, Multiple Dequeue, string data
        /// </summary>
        [TestMethod]
        public void TestQueue()
        {
            try
            {
                var queue = new Queue<int>(1);

                queue.Enqueue(1);
                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(4);
                queue.Enqueue(5);
                queue.Enqueue(6);
                queue.Enqueue(7);

                var dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 1);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 2);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 3);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 4);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 5);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 6);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 7);

                queue.Enqueue(8);
                Assert.IsTrue(queue.Peek() == 8);
                Assert.IsTrue(queue.Count == 1);
                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 8);
                Assert.IsTrue(queue.Count == 0);

                Assert.ThrowsException<InvalidOperationException>(() => queue = new Queue<int>(0));

                

                queue.Enqueue(1);
                Assert.IsTrue(queue.Count == 1);

                Assert.IsTrue(queue.Peek() == 1);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == 1);
                Assert.IsTrue(queue.Count == 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Multiple Enqueue, Multiple Dequeue, string data
        /// </summary>
        [TestMethod]
        public void TestQueueWithStringDataType()
        {
            try
            {
                var queue = new Queue<string>(1);

                queue.Enqueue("apple");
                queue.Enqueue("banana");
                queue.Enqueue("cherry");
                queue.Enqueue("date");
                queue.Enqueue("elderberry");

                var dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "apple");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "banana");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "cherry");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "date");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "elderberry");

                queue.Enqueue("fig");
                queue.Enqueue("grape");
                queue.Enqueue("honeydew");

                var peekElement = queue.Peek();
                Assert.IsTrue(peekElement == "fig");

                var count = queue.Count;
                Assert.IsTrue(count == 3);
                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "fig");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "grape");

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(dequeueElement == "honeydew");

                Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());


                count = queue.Count;
                Assert.IsTrue(count == 0);
            }
            catch (Exception ex)
            {
                Assert.Fail("An unexpected exception was thrown: " + ex.Message);
            }
        }

        /// <summary>
        /// Person Class to Enqueue (as a element)
        /// </summary>
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }

            public Person(string name, int age)
            {
                Name = name;
                Age = age;
            }
        }

        /// <summary>
        /// Checking Equality of one or more objects
        /// </summary>
        /// <typeparam name="T"> Generic Type </typeparam>
        /// <param name="obj1"> instance 1 </param>
        /// <param name="obj2"> instance 2 </param>
        /// <returns></returns>
        public static bool AreEqual<T>(T obj1, T obj2)
        {
            if (obj1 == null && obj2 == null)
            {
                return true;
            }

            if (obj1 == null || obj2 == null)
            {
                return false;
            }

            if (obj1.GetType() != obj2.GetType())
            {
                return false;
            }

            var properties = obj1.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                var value1 = property.GetValue(obj1);
                var value2 = property.GetValue(obj2);

                if (value1 == null && value2 == null)
                {
                    continue;
                }

                if (value1 == null || value2 == null)
                {
                    return false;
                }

                if (!value1.Equals(value2))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// testing with instance/object
        /// </summary>
        [TestMethod]
        public void TestQueueWithCustomObjectDataType()
        {
            try
            {
                var queue = new Queue<Person>(1);

                var person1 = new Person("Alice", 25);
                var person2 = new Person("Bob", 30);
                var person3 = new Person("Charlie", 35);
                queue.Enqueue(person1);
                queue.Enqueue(person2);
                queue.Enqueue(person3);

                var dequeueElement = queue.Dequeue();
                Assert.IsTrue(AreEqual(person1, dequeueElement));

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(AreEqual(person2, dequeueElement));

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(AreEqual(person3, dequeueElement));

                queue.Enqueue(new Person("Dave", 40));
                queue.Enqueue(new Person("Eve", 45));

                var peekElement = queue.Peek();
                Assert.IsTrue(AreEqual(peekElement, new Person("Dave", 40)));

                var count = queue.Count;
                Assert.IsTrue(count == 2);

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(AreEqual(dequeueElement, new Person("Dave", 40)));

                dequeueElement = queue.Dequeue();
                Assert.IsTrue(AreEqual(dequeueElement, new Person("Eve", 45)));


                Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());

                count = queue.Count;
                Assert.IsTrue(count == 0);
            }
            catch (Exception ex)
            {
                Assert.Fail("An unexpected exception was thrown: " + ex.Message);
            }
        }

        /// <summary>
        /// checking initialisation
        /// </summary>
        [TestMethod]
        public void TestInitialization()
        {
            Queue<string> queue = new Queue<string>(10);

            Assert.AreEqual(0, queue.Count);
            Assert.AreEqual(0, queue.InMemoryCount);
            Assert.AreEqual(0, queue.OnDiskCount);

        }


        /// <summary>
        /// checking initialisation
        /// </summary>
        [TestMethod]
        public void TestInMeMoryAndDiskCount1()
        {
            Queue<string> queue = new Queue<string>(10);

            queue.Enqueue("item1");

            Assert.AreEqual(1, queue.Count);
            Assert.AreEqual(1, queue.InMemoryCount);
            Assert.AreEqual(0, queue.OnDiskCount);
            Assert.AreEqual("item1", queue.Peek());

        }

        /// <summary>
        /// checking initialisation
        /// </summary>
        [TestMethod]
        public void TestInMeMoryAndDiskCount2()
        {
            Queue<string> queue = new Queue<string>(1);

            queue.Enqueue("item1");
            queue.Enqueue("item2");

            Assert.AreEqual(2, queue.Count);
            Assert.AreEqual(1, queue.InMemoryCount);
            Assert.AreEqual(1, queue.OnDiskCount);
            Assert.AreEqual("item1", queue.Peek());
        }

        /// <summary>
        /// checking initialisation
        /// </summary>
        [TestMethod]
        public void TestMultipleDataType()
        {
            Queue<object> queue = new Queue<object>(5);
            DateTime timestamp = DateTime.Now;
            queue.Enqueue(1);
            queue.Enqueue("Hello");
            queue.Enqueue(timestamp);
            queue.Enqueue(3.14);
            queue.Enqueue(true);

            Assert.AreEqual(1, queue.Dequeue());
            Assert.AreEqual("Hello", queue.Dequeue());
            Assert.AreEqual(timestamp, queue.Dequeue());
            Assert.AreEqual(3.14, queue.Dequeue());
            Assert.AreEqual(true, queue.Dequeue());

        }

        /// <summary>
        /// checking  multiple operations
        /// </summary>
        [TestMethod]
        public void TestMultipleEnqueueAndDequeue()
        {
            Queue<int> queue = new Queue<int>(5);
            for (int i = 0; i < 5; i++)
            {
                queue.Enqueue(i);
            }

            Assert.AreEqual(5, queue.InMemoryCount);
            Assert.AreEqual(0, queue.OnDiskCount);
            Assert.AreEqual(5, queue.Count);
            for (int i = 5; i < 10; i++)
            {
                queue.Enqueue(i);
            }

            Assert.AreEqual(5, queue.InMemoryCount);
            Assert.AreEqual(5, queue.OnDiskCount);
            Assert.AreEqual(10, queue.Count);

            for (int i = 0; i < 3; i++)
            {
                queue.Dequeue();
            }

            Assert.AreEqual(5, queue.InMemoryCount);
            Assert.AreEqual(2, queue.OnDiskCount);


        }


        [TestMethod]
        public void TestEnqueueAndDequeue()
        {
            int maxInMemorySize = 2;
            Queue<string> queue1 = new Queue<string>(maxInMemorySize);
            Queue<int> queue2 = new Queue<int>(maxInMemorySize);

            queue1.Enqueue("hello");
            queue2.Enqueue(123);
            queue1.Enqueue("world");
            queue2.Enqueue(456);

            Assert.AreEqual("hello", queue1.Dequeue());
            Assert.AreEqual(123, queue2.Dequeue());
            Assert.AreEqual("world", queue1.Dequeue());
            Assert.AreEqual(456, queue2.Dequeue());
        }

        [TestMethod]
        public void TestPeek2()
        {
            int maxInMemorySize = 2;
            Queue<int> queue = new Queue<int>(maxInMemorySize);
            queue.Enqueue(123);
            queue.Enqueue(456);

            int peek1 = queue.Peek();
            queue.Dequeue();
            int peek2 = queue.Peek();

            Assert.AreEqual(123, peek1);
            Assert.AreEqual(456, peek2);
        }

        [TestMethod]
        public void TestCount()
        {
            int maxInMemorySize = 2;
            Queue<int> queue = new Queue<int>(maxInMemorySize);

            int count1 = queue.Count;
            queue.Enqueue(123);
            int count2 = queue.Count;
            queue.Enqueue(456);
            int count3 = queue.Count;
            queue.Dequeue();
            int count4 = queue.Count;

            Assert.AreEqual(0, count1);
            Assert.AreEqual(1, count2);
            Assert.AreEqual(2, count3);
            Assert.AreEqual(1, count4);
        }

        [TestMethod]
        public void TestOnDiskCount()
        {
            int maxInMemorySize = 2;
            Queue<int> queue = new Queue<int>(maxInMemorySize);

            int onDiskCount1 = queue.OnDiskCount;
            queue.Enqueue(123);
            int onDiskCount2 = queue.OnDiskCount;
            queue.Enqueue(456);
            int onDiskCount3 = queue.OnDiskCount;
            queue.Enqueue(789);
            int onDiskCount4 = queue.OnDiskCount;

            Assert.AreEqual(0, onDiskCount1);
            Assert.AreEqual(0, onDiskCount2);
            Assert.AreEqual(0, onDiskCount3);
            Assert.AreEqual(1, onDiskCount4);
        }

        [TestMethod]
        public void TestInMemoryCount()
        {
            int maxInMemorySize = 2;
            Queue<int> queue = new Queue<int>(maxInMemorySize);

            int inMemoryCount1 = queue.InMemoryCount;
            queue.Enqueue(123);
            int inMemoryCount2 = queue.InMemoryCount;
            queue.Enqueue(456);
            int inMemoryCount3 = queue.InMemoryCount;
            queue.Enqueue(789);
            int inMemoryCount4 = queue.InMemoryCount;

            Assert.AreEqual(0, inMemoryCount1);
            Assert.AreEqual(1, inMemoryCount2);
            Assert.AreEqual(2, inMemoryCount3);
            Assert.AreEqual(2, inMemoryCount4);
        }
        /// <summary>
        /// checking  Thread safe
        /// </summary>
        [TestMethod]
        public void TestThreadSafe()
        {
            var queue = new Queue<int>(2);

            queue.Enqueue(1);
            queue.Enqueue(2);
            var thread = new Thread(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    queue.Enqueue(i);
                }
            });
            thread.Start();
            thread.Join();
            Assert.AreEqual(102, queue.Count);


        }

        /// <summary>
        /// checking  Thread safe
        /// </summary>
        [TestMethod]
        public void TestManyElements1()
        {

            Queue<int> queue = new Queue<int>(2);
            for (int i = 0; i < 10; i++)
            {
                queue.Enqueue(i);
            }

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(i, queue.Peek());
                Assert.AreEqual(i, queue.Dequeue());
            }

        }

        /// <summary>
        /// checking  Thread safe
        /// </summary>
        [TestMethod]
        public void TestManyElements2()
        {

            Queue<int> queue = new Queue<int>(2);
            for (int i = 0; i < 10; i++)
            {
                queue.Enqueue(i);
            }

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(i, queue.Dequeue());
                if (i + 1 < 10)
                    Assert.AreEqual(i + 1, queue.Peek());
                else
                    Assert.ThrowsException<InvalidOperationException>(() => queue.Peek());

            }

        }


        [TestMethod]
        public void TestMultipleQueues()
        {
            Queue<int> q1 = new Queue<int>(1);
            Queue<string> q2 = new Queue<string>(1);
            Queue<Person1> q3 = new Queue<Person1>(1);
            int intValue = 42;
            string stringValue = "Hello, world!";
            Person1 personValue = new Person1 { Name = "Alice", Age = 30 };
            q1.Enqueue(intValue);
            q1.Enqueue(intValue);
            q1.Enqueue(intValue);
            q2.Enqueue(stringValue);
            q3.Enqueue(personValue);
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                int queueIndex = random.Next(0, 3);
                switch (queueIndex)
                {
                    case 0:
                        if (q1.Count== 3)
                        {
                            int dequeuedValue = q1.Dequeue();
                            Assert.AreEqual(intValue, dequeuedValue);
                            Assert.IsTrue(q1.Count == 2);
                            Assert.IsTrue(q1.InMemoryCount == 1);
                            Assert.IsTrue(q1.OnDiskCount == 1);
                        }
                        else
                        {
                            q1.Enqueue(intValue);
                        }
                        break;
                    case 1:
                        if (q2.Count > 0)
                        {
                            string dequeuedValue = q2.Dequeue();
                            Assert.AreEqual(stringValue, dequeuedValue);
                        }
                        else
                        {
                            q2.Enqueue(stringValue);
                        }
                        break;
                    case 2:
                        if (q3.Count > 0)
                        {
                            Person1 dequeuedValue = q3.Dequeue();
                            Assert.AreEqual(personValue, dequeuedValue);
                        }
                        else
                        {
                            q3.Enqueue(personValue);
                        }
                        break;
                }
            }
        }


        public class Person1
        {
            public string Name { get; set; }
            public int Age { get; set; }
            

        }


        [TestMethod]
        public void EnqueueAddsDataTableToQueue()
        {
            var queue = new Queue<DataTable>(1);

            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Rows.Add(1, "John Doe");

            queue.Enqueue(dataTable);

            Assert.AreEqual(1, queue.Count);
            Assert.AreEqual(dataTable, queue.Peek());
        }

        [TestMethod]
        public void DequeueRemovesDataTableFromQueue()
        {
            var queue = new Queue<DataTable>(1);

            var dataTable1 = new DataTable();
            dataTable1.Columns.Add("Id", typeof(int));
            dataTable1.Columns.Add("Name", typeof(string));
            dataTable1.Rows.Add(1, "John Doe");

            var dataTable2 = new DataTable();
            dataTable2.Columns.Add("Id", typeof(int));
            dataTable2.Columns.Add("Name", typeof(string));
            dataTable2.Rows.Add(2, "Jane Doe");
            queue.Enqueue(dataTable1);
            queue.Enqueue(dataTable2);
            dataTable2 = new DataTable();
            dataTable2.Columns.Add("Id", typeof(int));
            dataTable2.Columns.Add("Name", typeof(string));
            dataTable2.Rows.Add(2, "Jane Doe");
            queue.Enqueue(dataTable2);

            dataTable2 = new DataTable();
            dataTable2.Columns.Add("Id", typeof(int));
            dataTable2.Columns.Add("Name", typeof(string));
            dataTable2.Rows.Add(2, "Jane Doe");
            queue.Enqueue(dataTable2);

            dataTable2 = new DataTable();
            dataTable2.Columns.Add("Id", typeof(int));
            dataTable2.Columns.Add("Name", typeof(string));
            dataTable2.Rows.Add(2, "Jane Doe");
            queue.Enqueue(dataTable2);


            var dequeuedDataTable = queue.Dequeue();
            Assert.AreEqual(4, queue.Count);
            Assert.AreEqual(3, queue.OnDiskCount);
            Assert.AreEqual(1, queue.InMemoryCount);
            Assert.AreEqual(dataTable1, dequeuedDataTable);
            queue.Dequeue();
            queue.Dequeue();
            queue.Dequeue();
            Assert.AreEqual(1, queue.Count);
            Assert.AreEqual(0, queue.OnDiskCount);
            Assert.AreEqual(1, queue.InMemoryCount);


            dataTable2 = new DataTable();
            dataTable2.Columns.Add("Id", typeof(int));
            dataTable2.Columns.Add("Name", typeof(string));
            dataTable2.Rows.Add(2, "Jane Doe");
            queue.Enqueue(dataTable2);

            dataTable2 = new DataTable();
            dataTable2.Columns.Add("Id", typeof(int));
            dataTable2.Columns.Add("Name", typeof(string));
            dataTable2.Rows.Add(2, "Jane Doe");
            queue.Enqueue(dataTable2);

            dataTable2 = new DataTable();
            dataTable2.Columns.Add("Id", typeof(int));
            dataTable2.Columns.Add("Name", typeof(string));
            dataTable2.Rows.Add(2, "Jane Doe");
            queue.Enqueue(dataTable2);

            Assert.AreEqual(4, queue.Count);
            Assert.AreEqual(3, queue.OnDiskCount);
            Assert.AreEqual(1, queue.InMemoryCount);
            Assert.AreEqual(dataTable1, dequeuedDataTable);
            queue.Dequeue();
            queue.Dequeue();
            queue.Dequeue();
            Assert.AreEqual(1, queue.Count);
            Assert.AreEqual(0, queue.OnDiskCount);
            Assert.AreEqual(1, queue.InMemoryCount);


        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestAddRemove()
        {
            Queue<int> queue1 = new Queue<int>(5);
            Queue<int> queue2 = new Queue<int>(5);
            Queue<int> queue3 = new Queue<int>(5);
            Queue<int> queue4 = new Queue<int>(5);
            Queue<int> queue5 = new Queue<int>(5);
            for (int i = 0; i < 25; i++)
            {
                queue1.Enqueue(i);
                int num = i + 1;
                Assert.AreEqual(num, queue1.Count);
                if (num > 5)
                {
                    Assert.AreEqual(num - 5, queue1.OnDiskCount);
                    Assert.AreEqual(5, queue1.InMemoryCount);
                }
                else
                {
                    Assert.AreEqual(0, queue1.OnDiskCount);
                    Assert.AreEqual(num, queue1.InMemoryCount);
                }

                queue2.Enqueue(i + 25);
                Assert.AreEqual(num, queue2.Count);
                if (num > 5)
                {
                    Assert.AreEqual(num - 5, queue2.OnDiskCount);
                    Assert.AreEqual(5, queue2.InMemoryCount);
                }
                else
                {
                    Assert.AreEqual(0, queue2.OnDiskCount);
                    Assert.AreEqual(num, queue2.InMemoryCount);
                }

                queue3.Enqueue(i + 50);
                Assert.AreEqual(num, queue3.Count);
                if (num > 5)
                {
                    Assert.AreEqual(num - 5, queue3.OnDiskCount);
                    Assert.AreEqual(5, queue3.InMemoryCount);
                }
                else
                {
                    Assert.AreEqual(0, queue3.OnDiskCount);
                    Assert.AreEqual(num, queue3.InMemoryCount);
                }

                queue4.Enqueue(i + 75);
                Assert.AreEqual(num, queue4.Count);
                if (num > 5)
                {
                    Assert.AreEqual(num - 5, queue4.OnDiskCount);
                    Assert.AreEqual(5, queue4.InMemoryCount);
                }
                else
                {
                    Assert.AreEqual(0, queue4.OnDiskCount);
                    Assert.AreEqual(num, queue4.InMemoryCount);
                }

                queue5.Enqueue(i + 100);
                Assert.AreEqual(num, queue5.Count);
                if (num > 5)
                {
                    Assert.AreEqual(num - 5, queue5.OnDiskCount);
                    Assert.AreEqual(5, queue5.InMemoryCount);
                }
                else
                {
                    Assert.AreEqual(0, queue5.OnDiskCount);
                    Assert.AreEqual(num, queue5.InMemoryCount);
                }
            }
            for (int i = 0; i < 25; i++)
            {
                int num = 25-i-1;
                Assert.AreEqual(i, queue1.Dequeue());
                Assert.AreEqual(num, queue1.Count);
                if (num > 5)
                {
                    Assert.AreEqual(num - 5, queue1.OnDiskCount);
                    Assert.AreEqual(5, queue1.InMemoryCount);
                }
                else
                {
                    Assert.AreEqual(0, queue1.OnDiskCount);
                    Assert.AreEqual(num, queue1.InMemoryCount);
                }

                Assert.AreEqual(i + 25, queue2.Dequeue());
                Assert.AreEqual(num, queue2.Count);
                if (num > 5)
                {
                    Assert.AreEqual(num - 5, queue2.OnDiskCount);
                    Assert.AreEqual(5, queue2.InMemoryCount);
                }
                else
                {
                    Assert.AreEqual(0, queue2.OnDiskCount);
                    Assert.AreEqual(num, queue2.InMemoryCount);
                }

                Assert.AreEqual(i + 50, queue3.Dequeue());
                Assert.AreEqual(num, queue3.Count);
                if (num > 5)
                {
                    Assert.AreEqual(num - 5, queue3.OnDiskCount);
                    Assert.AreEqual(5, queue3.InMemoryCount);
                }
                else
                {
                    Assert.AreEqual(0, queue3.OnDiskCount);
                    Assert.AreEqual(num, queue3.InMemoryCount);
                }

                Assert.AreEqual(i + 75, queue4.Dequeue());
                Assert.AreEqual(num, queue4.Count);
                if (num > 5)
                {
                    Assert.AreEqual(num - 5, queue4.OnDiskCount);
                    Assert.AreEqual(5, queue4.InMemoryCount);
                }
                else
                {
                    Assert.AreEqual(0, queue4.OnDiskCount);
                    Assert.AreEqual(num, queue4.InMemoryCount);
                }

                Assert.AreEqual(i + 100, queue5.Dequeue());
                Assert.AreEqual(num, queue5.Count);
                if (num > 5)
                {
                    Assert.AreEqual(num - 5, queue5.OnDiskCount);
                    Assert.AreEqual(5, queue5.InMemoryCount);
                }
                else
                {
                    Assert.AreEqual(0, queue5.OnDiskCount);
                    Assert.AreEqual(num, queue5.InMemoryCount);
                }
            }
        }


    }
}
