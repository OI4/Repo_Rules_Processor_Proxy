from secrets.url_details import base_url, submodel_id, header
import requests
import json

class AASPropertyService:
    
    def get_property_value(property_name):
        response = requests.get(f"{base_url}/submodels/{submodel_id}/submodel-elements/{property_name}/$value", headers=header)
        if response.status_code == 200:
            return json.loads(response.content)
        else:
            raise Exception(f"Failed to get property value: {response.status_code} - {response.text}")

    def update_property(property_name, property_value):
        url_string = f"{base_url}/submodels/{submodel_id}/submodel-elements/{property_name}/$value"
        print(f"PATCH request to {url_string} with value: {json.dumps(property_value)}")
        response = requests.patch(url_string, data=json.dumps(property_value), headers=header)
        if response.status_code != 204:
            raise Exception(f"Failed to set property value: {response.status_code} - {response.text}")