from flask import Flask, jsonify, make_response, request
from flask_cors import CORS, cross_origin
from pymongo import MongoClient
import os
import socket

mongo_url = os.getenv('TODOAPP_MongoUrl', 'mongodb://mongodb:27017')
mongo = MongoClient(mongo_url)

app = Flask(__name__)
cors = CORS(app)
app.config['CORS_HEADERS'] = 'Content-Type'

# List all users
@app.route("/api/users")
@cross_origin()
def get_all_users():
    users_collection = mongo.todoapp.users
    output = []
    for u in users_collection.find():
        output.append({'name': u['name'], 'id': u['id']})
    return jsonify(output)


# Get a particular user
@app.route("/api/users/<int:id>")
@cross_origin()
def get_user(id):
    users_collection = mongo.todoapp.users
    u = users_collection.find_one({'id': id})
    if u:
        output = {'name': u['name'], 'id': u['id']}
        return jsonify(output)
    else:
        return make_response(jsonify({'error': 'Not found'}), 404)


# Decide if the particual call is authenticated; used by forward authentication
@app.route("/api/auth")
def get_is_auth():
    # return make_response(jsonify({'error': 'Unauthorized'}), 401)
    return make_response(jsonify({'auth': 'ok'}), 200)


if __name__ == "__main__":

    # Add sample data
    users_collection = mongo.todoapp.users
    users_collection.replace_one({'id': 1}, {'id': 1, 'name': 'Alma'}, True)
    users_collection.replace_one({'id': 2}, {'id': 2, 'name': 'Banan'}, True)
    users_collection.replace_one({'id': 3}, {'id': 3, 'name': 'Citrom'}, True)

    # Start the Flash host
    app.run(host='0.0.0.0', port=80)
