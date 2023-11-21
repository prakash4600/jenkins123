pipeline {
    agent any

    environment {
        // Define your ACR information
        ACR_NAME = 'MallikharjunaReddy'
        RESOURCE_GROUP = 'MallikharjunaReddy'
        AZURE_CREDENTIALS_ID = 'a37d4b17-57b1-4bd8-984e-07b8a42d4547' // Jenkins credential ID for Azure Service Principal
        DOCKER_IMAGE_NAME = 'jenkins'
        DOCKERFILE_PATH = '/Dockerfile'
    }

    stages {
        stage('Checkout') {
            steps {
                // Assuming your source code is in a Git repository
                checkout scm
            }
        }

        stage('Build Docker Image') {
            steps {
                script {
                    // Build the Docker image
                    docker.build("${DOCKER_IMAGE_NAME}:${env.BUILD_ID}", "-f ${DOCKERFILE_PATH} .")
                }
            }
        }

        stage('Azure Login') {
            steps {
                script {
                    // Login to Azure using Service Principal
                    azureLogin()
                }
            }
        }

        stage('Push to ACR') {
            steps {
                script {
                    // Tag the Docker image for ACR
                    def acrServer = "${ACR_NAME}.azurecr.io"
                    def dockerImageTag = "${acrServer}/${DOCKER_IMAGE_NAME}:${env.BUILD_ID}"
                    docker.image("${DOCKER_IMAGE_NAME}:${env.BUILD_ID}").tag(dockerImageTag)

                    // Push the Docker image to ACR
                    docker.withRegistry("https://${acrServer}", "${AZURE_CREDENTIALS_ID}") {
                        docker.image(dockerImageTag).push()
                    }
                }
            }
        }
    }

    post {
        success {
            echo 'Docker image successfully pushed to Azure Container Registry!'
        }
    }
}

def azureLogin() {
    withCredentials([azureServicePrincipal("${AZURE_CREDENTIALS_ID}")]) {
        sh "az login --service-principal --username 'a37d4b17-57b1-4bd8-984e-07b8a42d4547' --password 'w-x8Q~GiNh.0tdrbx8_aqn6ImQxiwAte7hb8Ickc' --tenant '89627a4e-a49f-415a-b6fd-acd74f822cb6' "
    }
}
