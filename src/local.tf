terraform {
  required_providers {
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = ">= 2.0.0"
    }
  }
}

provider "kubernetes" {
  config_path = "~/.kube/config"
}

resource "kubernetes_namespace" "identity" {
  metadata {
    name = "identity-business"
  }
}

resource "kubernetes_deployment" "identity" {
  metadata {
    name = "identity-business"
    namespace = kubernetes_namespace.identity.metadata.0.name
    labels = {
      app = "IdentityBusiness"
    }
  }

  spec {
    replicas = 2
    selector {
      match_labels = {
        app = "identity-business"
      }
    }
    template {
      metadata {
        labels = {
          app = "identity-business"
        }
      }
      spec {
        container {
          image = "identity-business:latest"
          name = "identity-business-container"
          image_pull_policy = "IfNotPresent"
          port {
            container_port = 80
            protocol = "TCP"
          }
        }
      }
    }
  }
}