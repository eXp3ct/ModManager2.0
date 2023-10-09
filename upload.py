import xml.etree.ElementTree as ET
import requests
import json
import subprocess

# Replace the following values with your data
workspace = "mmmodmanager"
repo_slug = "updates"

class AutoUpdaterOptions:
    def __init__(self, api_key):
        self.ApiKey = api_key

# Загрузите appsettings.json и получите значение api_token
def load_appsettings():
    try:
        with open("Expect.ModManager.View/appsettings.json", "r") as file:
            appsettings = json.load(file)
            api_key = appsettings.get("AutoUpdaterOptions", {}).get("ApiKey")
            return api_key
    except Exception as e:
        print(f"Error loading appsettings.json: {e}")
        return None

# Загрузка api_token из appsettings.json
api_token = load_appsettings()

# URL for uploading files to BitBucket
url = f"https://api.bitbucket.org/2.0/repositories/{workspace}/{repo_slug}/downloads"

def upload_to_bitbucket():
    # Headers with Bearer Token authorization
    headers = {
        "Authorization": f"Bearer {api_token}"
    }

    # Upload updates.xml
    updates_xml_file = "updates.xml"
    with open(updates_xml_file, "rb") as file:
        files = {"files": (updates_xml_file, file)}
        response = requests.post(url, headers=headers, files=files)

    # Check the success of updates.xml upload
    if response.status_code == 201:
        print("updates.xml was successfully uploaded to BitBucket")
    else:
        print(f"Error uploading updates.xml: {response.status_code}")
        print(response.text)

    # Upload release.zip
    release_zip_file = "release.zip"
    with open(release_zip_file, "rb") as file:
        files = {"files": (release_zip_file, file)}
        response = requests.post(url, headers=headers, files=files)

    # Check the success of release.zip upload
    if response.status_code == 201:
        print("release.zip was successfully uploaded to BitBucket")
    else:
        print(f"Error uploading release.zip: {response.status_code}")
        print(response.text)


def get_file_version(file_path):
    try:
        # Используем команду powershell для получения версии файла
        command = f"(Get-Item '{file_path}').VersionInfo.FileVersion"
        result = subprocess.check_output(["powershell", command], universal_newlines=True).strip()

        if result:
            return result
        else:
            print(f"Не удалось получить версию файла: {file_path}")
            return None
    except Exception as e:
        print(f"Ошибка при получении версии файла: {e}")
        return None
    
def update_updates_xml(updates_xml_path, new_version):
    try:
        # Загрузить XML-файл
        tree = ET.parse(updates_xml_path)
        root = tree.getroot()

        # Найти элемент с тэгом <version> и обновить его значение
        for version_elem in root.findall(".//version"):
            version_elem.text = new_version

        # Сохранить изменения в XML-файле
        tree.write(updates_xml_path)

        print(f"Version in file {updates_xml_path} successfully changed to {new_version}")
    except Exception as e:
        print(f"Error occured while changing verion in file {updates_xml_path}: {e}")

path = "Expect.ModManager.View/bin/Debug/net7.0-windows/ModManager.exe"
version = get_file_version(path)

xmlPath = "updates.xml"
update_updates_xml(xmlPath, version)

upload_to_bitbucket()
