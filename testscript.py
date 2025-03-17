import requests
import json
import threading
import time
import random
import argparse

# GraphQL endpoint
url = 'https://localhost:5001/graphql'

# GraphQL Mutation
mutation = """
mutation UpdateUserLocation($userId: UUID!, $userInput: UserLocationTypeInput!) {
  updateUserLocation(
    userId: $userId
    userInput: $userInput
  ) {
    createdTimestamp
    latitude
    longitude
    name
    updatedTimestamp
    userId
  }
}
"""

# Function to update user location
def update_location(user_id, user_name):
    # Simulate new location updates
    latitude = 40.000 + random.uniform(-0.01, 0.01)  # Randomly changing latitude
    longitude = 152.4194 + random.uniform(-0.01, 0.01)  # Randomly changing longitude

    # Variables for the request
    variables = {
        "userId": user_id,
        "userInput": {
            "name": user_name,
            "latitude": latitude,
            "longitude": longitude
        }
    }

    # Headers for the request
    headers = {
        "Content-Type": "application/json",
        # Add any required authentication headers if needed
    }

    try:
        # Send the request (disable SSL verification for development)
        response = requests.post(
            url,
            json={'query': mutation, 'variables': variables},
            headers=headers,
            verify=False  # Disable SSL verification (only for development)
        )

        # Handle the response
        if response.status_code == 200:
            print(f"Location updated: {latitude}, {longitude}")
            print(json.dumps(response.json(), indent=2))  # Pretty print the JSON response
        else:
            print(f"Error: {response.status_code} - {response.text}")
    except requests.exceptions.RequestException as e:
        print(f"Request failed: {e}")

# Function to run multiple updates in parallel using threads
def run_parallel_updates(user_id, user_name, num_updates=10):
    threads = []
    for _ in range(num_updates):
        # Create a new thread for each location update, passing user_id and user_name
        thread = threading.Thread(target=update_location, args=(user_id, user_name))
        threads.append(thread)
        thread.start()
        time.sleep(1)  # Small delay to avoid overwhelming the server

    # Wait for all threads to finish
    for thread in threads:
        thread.join()

# Main function to handle argument parsing
def main():
    # Set up command-line argument parser
    parser = argparse.ArgumentParser(description="Update user location via GraphQL.")
    
    # Adding the user_id and user_name arguments
    parser.add_argument('user_id', type=str, help='User ID (UUID format)')
    parser.add_argument('user_name', type=str, help='User name')

    # Parse arguments from command line
    args = parser.parse_args()

    # Run the parallel updates using the passed arguments
    run_parallel_updates(args.user_id, args.user_name, num_updates=10)  # Update 10 times in parallel (you can change this number)

# Run the script
if __name__ == "__main__":
    main()
