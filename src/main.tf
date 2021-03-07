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
    name = var.src_name
  }
}

resource "kubernetes_secret" "identity" {
  metadata {
    name      = var.secret_name
    namespace = kubernetes_namespace.identity.metadata.0.name
  }
  data = {
    Broker_Host                       = var.broker_connection_string
    DatabaseSettings_ConnectionString = var.db_connection_string
    DatabaseSettings_DatabaseName     = var.db_name
    IdentityServer_OrgUrl             = var.identity_server_org_url
    IdentityServer_ClientId           = var.identity_server_client_id
  }
}

resource "kubernetes_deployment" "identity" {
  metadata {
    name      = var.src_name
    namespace = kubernetes_namespace.identity.metadata.0.name
    labels = {
      app = var.deployment_label
    }
  }

  spec {
    replicas = var.replicas_count
    selector {
      match_labels = {
        app = var.src_name
      }
    }
    template {
      metadata {
        labels = {
          app = var.src_name
        }
      }
      spec {
        container {
          image             = "${var.src_name}:latest"
          name              = "${var.src_name}-container"
          image_pull_policy = "IfNotPresent"
          resources {
            limits = {
              cpu    = "0.5"
              memory = "512Mi"
            }
          }
          port {
            container_port = 80
            protocol       = "TCP"
          }
          env_from {
            secret_ref {
              name = var.secret_name
            }
          }
        }
      }
    }
  }
}