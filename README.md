# StorageProblem

Make container management a minigame.

The heavier a container can hold, the less slots it has.

If **No Problem Mode** is enabled, the slot limit simply reflects the weight capacity of the container (**WeightCapacity of the container** x **ContainerSlotLimitRatio** ).

Otherwise, the slot limit is calculated by **MaxSlots** - (**(WeightCapacity of the container** - **BaseWeightCapacity)** x **Container Slot Limit Ratio**).

For example a 20kg container gets `12 - (20 - 10) * 0.2` = 10 slots, while a 40kg container gets `12 - (40 - 10) * 0.2` = 6 slots. slots.