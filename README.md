# StorageProblem

Make container management a minigame.

Every container has a **S** value which can be calculated by (**(`WeightCapacity of the container`** - **`BaseWeightCapacity`)** x **`Container Slot Limit Ratio`**).

Say, a 20kg container gets a **S** of `(20 - 10) * 0.2` = 2, and a 40kg container gets a **S** of `(40 - 10) * 0.2` = 6.

By default, a container can hold up to **`MaxSlots`** - **`S`**. So a 20kg container gets `12 - 2` = 10 slots, while a 40kg container gets `12 - 6` = 6 slots.

If **No Problem Mode** is enabled, **`S`** is simply used as the slot limit, so the heavier a container supports the more slots it has.

