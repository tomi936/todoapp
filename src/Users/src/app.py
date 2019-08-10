from flask import Flask, jsonify, make_response, request
from flask_cors import CORS, cross_origin
from pymongo import MongoClient
import os
import socket

# Development and production behavior might vary; decide runtime
is_development = os.environ.get('TODOAPP_IsDevelopment') is not None

# Get connection string from environment variable
mongo_url = os.getenv('TODOAPP_MongoUrl', 'mongodb://mongodb:27017')
mongo = MongoClient(mongo_url)

app = Flask(__name__)

if is_development:
    # Allow CORS; makes frontend development easier; must not be used in production
    cors = CORS(app)
    app.config['CORS_HEADERS'] = 'Content-Type'

# List all users
@app.route("/api/users")
@cross_origin()
def get_all_users():
    # TODO 3. feladat
    return jsonify( { [ {'name': 'name', 'id': 1} ] } )


# Get a particular user
@app.route("/api/users/<int:id>")
@cross_origin()
def get_user(id):
    # TODO 3. feladat
    return make_response(jsonify({'error': 'Not found'}), 404)


# Decide if the caller is authenticated; used by forward authentication
@app.route("/api/auth")
def get_is_auth():
    # TODO 6. feladat
    # return make_response(jsonify({'error': 'Unauthorized'}), 401)
    return make_response(jsonify({'auth': 'ok'}), 200)


if __name__ == "__main__":

    # Add sample data
    if is_development:
        users_collection = mongo.todoapp.users
        users_collection.replace_one({'id': 1}, {'id': 1, 'name': 'Alma'}, True)
        users_collection.replace_one({'id': 2}, {'id': 2, 'name': 'Banan'}, True)
        users_collection.replace_one({'id': 3}, {'id': 3, 'name': 'Citrom'}, True)

    # Start the Flash host
    app.run(host='0.0.0.0', port=80)
