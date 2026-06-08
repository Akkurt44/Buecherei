terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region = var.aws_region
}



resource "aws_key_pair" "buecherei_key" {
  key_name   = var.key_name
  public_key = file("~/.ssh/buecherei-key.pub")
}

resource "aws_security_group" "buecherei_sg" {
  name = "buecherei-sg"

  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 8000
    to_port     = 8000
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_instance" "buecherei_server" {
  ami                    = "ami-08eb150f611ca277f"
  instance_type          = var.instance_type
  key_name               = aws_key_pair.buecherei_key.key_name
  vpc_security_group_ids = [aws_security_group.buecherei_sg.id]

  tags = {
    Name = "buecherei-server"
  }
}
