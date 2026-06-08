output "ec2_public_ip" {
  value = aws_instance.buecherei_server.public_ip
}
