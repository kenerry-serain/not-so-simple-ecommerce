import { PrimaryColumn, Column, Entity } from "typeorm";

@Entity("Order")
export class Order{
    @PrimaryColumn()
    Id: number;

    @Column()
    Status: number;
}