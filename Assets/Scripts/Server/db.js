// Install first: npm install mongodb
import {MongoClient} from "mongodb";

// Replace this with the URI your friend gave you
const uri = "mongodb+srv://imperius:imperius2025@imperius.5qtvi1p.mongodb.net/?appName=imperius";

async function connectDB() {
    const client = new MongoClient(uri);
    try {
        await client.connect();
        console.log("Connected to MongoDB!");
        
        // Example: get a database
        const db = client.db("myDatabase");
        // Example: get a collection
        const collection = db.collection("myCollection");
        
        // Example: find documents
        const docs = await collection.find({}).toArray();
        console.log(docs);
    } catch (err) {
        console.error(err);
    } finally {
        await client.close();
    }
}

connectDB();
